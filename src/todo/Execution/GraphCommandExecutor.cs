using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Scoring;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;
using Todo.Extensions;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GraphCommandExecutor(IGraphHtmlTemplateProvider graphHtmlTemplateProvider,
    IGraphHtmlSubstitutionsMaker graphHtmlSubstitutionsMaker,
    IOutputWriter outputWriter, IScoresGenerator scoresGenerator,
    IConfigurationProvider configurationProvider, IDateAdjuster dateAdjuster,
    IScoreHtmlPathResolver scoreHtmlPathResolver, IDateAccessor dateAccessor,
    IHtmlFileLauncher htmlFileLauncher, ISpecialDateNamer specialDateNamer,
    IOrdinalHelper ordinalHelper)
    : CommandExecutorBase<GraphCommand>(outputWriter), IGraphCommandExecutor
{
    // === Chart dimensions & margins ===
    private const int Width = 960;
    private const int Height = 450;
    private const int MarginLeft = 80;
    private const int MarginRight = 220;   // space for legend
    private const int MarginTop = 60;
    private const int MarginBottom = 100;

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

    private string GenerateStackedBarChartHtml(ScoreInfo[] data)
    {
        if (data.Length == 0)
            return "<html class=\"light\"><body><h1>No data to display</h1></body></html>";

        var configuration = configurationProvider.ConfigInfo.Configuration;
        
        var scoreCategories = configuration.ScoreCategories;

        const int plotWidth = Width - MarginLeft - MarginRight;
        const int plotHeight = Height - MarginTop - MarginBottom;

        // === Calculate scaling ===
        var maxTotal = data.Max(d => (double)d.Total());

        if (maxTotal <= 0) maxTotal = 10;
        var yScale = plotHeight / maxTotal;

        // === Bar layout ===
        const string axisColour = "#333";
        const double axisLineWidth = 2.0;
        const double gapInTermsOfBarWidth = 0.4;
        var numBars = data.Length;
        var barWidth = (plotWidth - axisLineWidth) / ((1.0 + gapInTermsOfBarWidth) * numBars - gapInTermsOfBarWidth);
        var gap = barWidth * gapInTermsOfBarWidth;
        var totalBarSpace = numBars * (barWidth + gap) - gap;
        var startX = MarginLeft + axisLineWidth / 2.0; // + (plotWidth - totalBarSpace) / 2.0;

        // === SVG elements collection ===
        var elements = new List<XElement>
        {
            // Y-axis line
            new("line",
                new XAttribute("x1", MarginLeft), new XAttribute("y1", MarginTop + plotHeight),
                new XAttribute("x2", MarginLeft), new XAttribute("y2", MarginTop - axisLineWidth / 2.0),
                new XAttribute("stroke", axisColour), new XAttribute("stroke-width", axisLineWidth)),
            // X-axis line
            new("line",
                new XAttribute("x1", MarginLeft), new XAttribute("y1", MarginTop + plotHeight),
                new XAttribute("x2", MarginLeft + plotWidth - axisLineWidth / 2.0), new XAttribute("y2", MarginTop + plotHeight),
                new XAttribute("stroke", axisColour), new XAttribute("stroke-width", axisLineWidth))
        };

        // === Draw bars (stacked) ===
        for (var i = 0; i < numBars; i++)
        {
            var day = data[i];
            var barX = startX + i * (barWidth + gap);

            double stackY = MarginTop + plotHeight - ( axisLineWidth / 2.0);   // start from bottom

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
                    new XAttribute("stroke-width", "0"));

                // Hover tooltip
                rect.Add(new XElement("title",
                    $"{day.FilePath.Date:MMM dd yyyy}\n{scoreCategory.Name}: {score}"));

                elements.Add(rect);
                stackY -= segmentHeight;
            }

            var dayLabelX = barX + barWidth / 2.0 + 5;
            const int dayLabelY = MarginTop + plotHeight + 10;

            var dayLabel = GetDayLabel(dayLabelX, dayLabelY, day.FilePath.Date);

            elements.Add(dayLabel);
        }
        
        var ticks = GetYTicks((int)maxTotal).ToArray();

        // === Y-axis ticks & labels ===
        for (var i = 0; i < ticks.Length; i++)
        {
            var value = ticks[i];
            var y = MarginTop + plotHeight - value * yScale;

            // tick line
            elements.Add(new XElement("line",
                new XAttribute("x1", MarginLeft - 8), new XAttribute("y1", y),
                new XAttribute("x2", MarginLeft), new XAttribute("y2", y),
                new XAttribute("stroke", axisColour), new XAttribute("stroke-width", axisLineWidth)));

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
            var scoreCategoryY = legendY + (scoreCategories.Length - i - 1) * 28;
            
            // color box
            elements.Add(new XElement("rect",
                new XAttribute("x", legendX),
                new XAttribute("y", scoreCategoryY),
                new XAttribute("width", "18"),
                new XAttribute("height", "18"),
                new XAttribute("fill", cat.GraphColor.ToHex())));

            // label
            elements.Add(new XElement("text",
                new XAttribute("x", legendX + 26),
                new XAttribute("y", scoreCategoryY + 14),
                new XAttribute("font-family", "Arial, sans-serif"),
                new XAttribute("font-size", "13"),
                cat.Name));
        }

        // === Root SVG ===
        var svg = new XElement("svg",
            new XAttribute("width", Width),
            new XAttribute("height", Height),
            new XAttribute("viewBox", $"0 0 {Width} {Height}"),
            new XAttribute("preserveAspectRatio", "xMidYMid meet"),
            elements.ToArray());
        
        var initialTheme = configuration.HtmlTheme.ToString().ToLower();
        
        var chartTitle = configuration.TodoListInfo.AppearInHtmlSummaries
            ? $"{configuration.TodoListInfo.Name} \u2014 {configuration.DefaultDayIntervalForGamify} day summary"
            : $"{configuration.DefaultDayIntervalForGamify} day summary";
        
        var htmlSubstitutions = GraphHtmlSubstitutions.Of(chartTitle, initialTheme, 
            svg.ToString(), $"{dateAccessor.GetNow():yy-MM-dd HH:mm:ss}");

        var template = graphHtmlTemplateProvider.GetTemplate();
        
        var html = graphHtmlSubstitutionsMaker.MakeSubstitutions(htmlSubstitutions, 
            template.FileContents);

        return html;
    }

    private IEnumerable<int> GetYTicks(int maxTotal)
    {
        var orderOfMagnitude = (int)Math.Log10(maxTotal);
        var @_1000 = (int)Math.Pow(10, orderOfMagnitude);
        var @_5000 = (int)(maxTotal / @_1000) * @_1000;
        
        for (int i = 0; i <= @_5000; i += @_1000)
        {
            yield return i;
        }

        yield return maxTotal;
    }

    private XElement GetDayLabel(double x, double y, DateOnly date)
    {
        var dayLabel = new XElement("text",
            new XAttribute("x", x),
            new XAttribute("y", y),
            new XAttribute("text-anchor", "end"),
            new XAttribute("font-family", "Arial, sans-serif"),
            new XAttribute("font-size", "13"),
            new XAttribute("transform", $"rotate(-45 {x} {y})"));

        if (configurationProvider.ConfigInfo.Configuration.UseNamesForDays &&
            specialDateNamer.TryGetSpecialName(date, out var dateName))
        {
            dayLabel.Add(new XText(dateName!));
        }
        else
        {
            dayLabel.Add(new XText(date.ToString("ddd d")));

            var ordinal = ordinalHelper.GetOrdinal(date.Day);

            var tspan = new XElement("tspan",
                new XAttribute("font-size", "9"),
                new XAttribute("baseline-shift", "super"),
                new XText(ordinal)
            );
            dayLabel.Add(tspan);

            dayLabel.Add(new XText($" {date.ToString("MMMM")}"));
        }

        return dayLabel;
    }
}
