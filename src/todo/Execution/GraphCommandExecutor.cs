using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Scoring;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Extensions;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GraphCommandExecutor(IOutputWriter outputWriter, IScoresGenerator scoresGenerator,
    IConfigurationProvider configurationProvider, IDateAdjuster dateAdjuster, 
    IScoreHtmlPathResolver scoreHtmlPathResolver, IDateAccessor dateAccessor, IHtmlFileLauncher htmlFileLauncher)
    : CommandExecutorBase<GraphCommand>(outputWriter), IGraphCommandExecutor
{
    // === Chart dimensions & margins ===
    private const int Width = 960;
    private const int Height = 620;
    private const int MarginLeft = 80;
    private const int MarginRight = 220;   // space for legend
    private const int MarginTop = 60;
    private const int MarginBottom = 80;
    
    public override void Execute(GraphCommand command)
    {
        var now = dateAdjuster.GetTodayWithMidnightAdjusted();
        var intervalDays = configurationProvider.ConfigInfo.Configuration.DefaultDayIntervalForGamify;
        var start = now.AddDays(-intervalDays);
      
        var scoreInfos = scoresGenerator.GetNonZeroScoresForDateInterval(start, now);
        
        var html = GenerateStackedBarChartHtml(scoreInfos);

        var filePathInfo = scoreHtmlPathResolver.GetFilePathFor("", FileTypeEnum.Html);
        
        File.WriteAllText(filePathInfo.Path, html);
        
        htmlFileLauncher.LaunchFiles(filePathInfo.Path);
    }

    private string GenerateStackedBarChartHtml(ScoreInfo[] data, string chartTitle = "Daily Activity Breakdown")
    {
        if (data.Length == 0) 
            return "<html><body><h1>No data to display</h1></body></html>";

        var scoreCategories = configurationProvider.ConfigInfo.Configuration.ScoreCategories;
        
        const int plotWidth = Width - MarginLeft - MarginRight;
        const int plotHeight = Height - MarginTop - MarginBottom;

        // === Calculate scaling ===
        var maxTotal = data.Max(d => d.Total()) * 1.1; // 10% padding
        
        if (maxTotal <= 0) maxTotal = 10;
        var yScale = plotHeight / maxTotal;

        // === Bar layout ===
        var numBars = data.Length;
        var barWidth = Math.Max(30, plotWidth / (numBars * 1.8));
        var gap = barWidth * 0.4;
        var totalBarSpace = numBars * (barWidth + gap) - gap;
        var startX = MarginLeft + (plotWidth - totalBarSpace) / 2;

        // === SVG elements collection ===
        var elements = new List<XElement>
        {
            // Y-axis line
            new("line",
                new XAttribute("x1", MarginLeft), new XAttribute("y1", MarginTop),
                new XAttribute("x2", MarginLeft), new XAttribute("y2", MarginTop + plotHeight),
                new XAttribute("stroke", "#333"), new XAttribute("stroke-width", "3")),
            // X-axis line
            new("line",
                new XAttribute("x1", MarginLeft), new XAttribute("y1", MarginTop + plotHeight),
                new XAttribute("x2", MarginLeft + plotWidth), new XAttribute("y2", MarginTop + plotHeight),
                new XAttribute("stroke", "#333"), new XAttribute("stroke-width", "3"))
        };

        
        // === Draw bars (stacked) ===
        for (var i = 0; i < numBars; i++)
        {
            var day = data[i];
            var barX = startX + i * (barWidth + gap);

            double stackY = MarginTop + plotHeight;   // start from bottom
            
            foreach (var scoreCategory in scoreCategories)
            {
                if (!day.TryGetScore(scoreCategory, out var score)) continue;
                
                if (score <= 0) continue;

                var segmentHeight = score * yScale;
                var segmentY = stackY - segmentHeight;
                
                var rect = new XElement("rect",
                    new XAttribute("x", barX),
                    new XAttribute("y", segmentY),
                    new XAttribute("width", barWidth),
                    new XAttribute("height", segmentHeight),
                    new XAttribute("fill", scoreCategory.GraphColor.ToHex()),
                    new XAttribute("stroke", "#ffffff"),
                    new XAttribute("stroke-width", "2"));

                // Hover tooltip
                rect.Add(new XElement("title", 
                    $"{day.FilePath.Date:MMM dd yyyy}\n{scoreCategory.Name}: {score}"));

                elements.Add(rect);
                stackY -= segmentHeight;
            }

            // Day label
            elements.Add(new XElement("text",
                new XAttribute("x", barX + barWidth / 2),
                new XAttribute("y", MarginTop + plotHeight + 35),
                new XAttribute("text-anchor", "middle"),
                new XAttribute("font-family", "Arial, sans-serif"),
                new XAttribute("font-size", "13"),
                day.FilePath.Date.ToString("MMM dd")));
        }

        // === Y-axis ticks & labels ===
        const int tickCount = 6;
        for (var i = 0; i <= tickCount; i++)
        {
            var value = i * (maxTotal / tickCount);
            var y = MarginTop + plotHeight - (value * yScale);

            // tick line
            elements.Add(new XElement("line",
                new XAttribute("x1", MarginLeft - 8), new XAttribute("y1", y),
                new XAttribute("x2", MarginLeft), new XAttribute("y2", y),
                new XAttribute("stroke", "#666"), new XAttribute("stroke-width", "2")));

            // label
            elements.Add(new XElement("text",
                new XAttribute("x", MarginLeft - 12),
                new XAttribute("y", y + 4),
                new XAttribute("text-anchor", "end"),
                new XAttribute("font-family", "Arial, sans-serif"),
                new XAttribute("font-size", "12"),
                value.ToString("0")));
        }

        // === Legend ===
        const double legendX = Width - MarginRight + 30;
        const double legendY = MarginTop + 30;
        
        elements.Add(new XElement("text",
            new XAttribute("x", legendX), new XAttribute("y", legendY - 15),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "15"), new XAttribute("font-weight", "bold"),
            "Categories"));
        
        for (var i = 0; i < scoreCategories.Length; i++)
        {
            var cat = scoreCategories[i];

            // color box
            elements.Add(new XElement("rect",
                new XAttribute("x", legendX),
                new XAttribute("y", legendY + i * 28),
                new XAttribute("width", "18"),
                new XAttribute("height", "18"),
                new XAttribute("fill", cat.GraphColor.ToHex())));

            // label
            elements.Add(new XElement("text",
                new XAttribute("x", legendX + 26),
                new XAttribute("y", legendY + i * 28 + 14),
                new XAttribute("font-family", "Arial, sans-serif"),
                new XAttribute("font-size", "13"),
                cat.Name));
        }

        // === Root SVG ===
        var svg = new XElement("svg",
            new XAttribute("width", Width),
            new XAttribute("height", Height),
            new XAttribute("viewBox", $"0 0 {Width} {Height}"),
            elements.ToArray());

        // === Full HTML (self-contained) ===
        return $@"<!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""utf-8"">
        <title>{chartTitle}</title>
        <style>
            body {{ font-family: Arial, sans-serif; margin: 30px; background: #f9f9f9; }}
            h1 {{ text-align: center; color: #333; }}
            svg {{ background: white; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }}
        </style>
    </head>
    <body>
        <h1>{chartTitle}</h1>
        {svg}
        <br/><br/>
        Written at: {dateAccessor.GetNow():yy-MM-dd HH:mm:ss}
    </body>
    </html>";
    }
}
