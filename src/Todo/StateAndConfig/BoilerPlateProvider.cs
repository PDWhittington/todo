using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.StateAndConfig;

public class BoilerPlateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IDateAccessor dateAccessor,
    IConstantsProvider constantsProvider,
    IConsoleTextFormatter consoleTextFormatter
) : IBoilerPlateProvider
{
    private static readonly string[] _buildArguments =
    [
        "BuildConfiguration",
        "DebugType",
        "OptimizationPreference",
        "PublishAot",
        "PublishDir",
        "PublishReadyToRun",
        "PublishReadyToRunComposite",
        "PublishSingleFile",
        "PublishTrimmed",
        "RuntimeIdentifier",
        "SelfContained",
        "SourceRevisionId",
        "TargetFramework",
        "TieredCompilation",
    ];

    private static readonly string[] _buildArgumentsForLogging =
    [
        "BuildConfiguration",
        "DebugType",
        "OptimizationPreference",
        "PublishAot",
        "PublishReadyToRun",
        "PublishReadyToRunComposite",
        "PublishSingleFile",
        "PublishTrimmed",
        "SelfContained",
        "TieredCompilation",
    ];
    
    private static readonly Lazy<UnitAndMultiplier[]> UnitsAndMultipliers = new(() => CreateUnits().ToArray());

    public string GetBoilerPlateForLogging()
    {
        var sb = new StringBuilder();
        MakeBoilerPlateForLogging(sb);
        return sb.ToString();
    }
    
    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    private void MakeBoilerPlateForLogging(StringBuilder sb)
    {
        /*
         * Version information
         */

        var buildTime = assemblyInformationProvider.GetBuildTime();

        sb.AppendLine("Version Information")
            .AppendLine(
                $"\tBuild time: {buildTime.ToString("yyyy-MM-dd HH:mm:ss")}{TimeAgoMessage(buildTime)}"
            );

        AddGitInformation(sb, false);

        sb.AppendLine();

        /*
         * Build information
         */

        sb.AppendLine("Build Information");

        foreach (var buildArgument in _buildArgumentsForLogging)
        {
            var buildArgumentValue = assemblyInformationProvider.GetMetadata(buildArgument);
            sb.AppendLine($"\t{buildArgument}: {buildArgumentValue ?? "Unknown"}");
        }

        sb.AppendLine();

        /*
         * Process, Framework and OS
         */

        sb.AppendLine("Process, Framework and OS")
            .Append($"\tDEBUG flag: {assemblyInformationProvider.DebugFlag()}");
        //Append not AppendLine because a fresh new line is added as this
        //string is written to the log file
    }
    
    public void MakeBoilerPlate(StringBuilder sb)
    {
        /*
         * Version information
         */

        var buildTime = assemblyInformationProvider.GetBuildTime();

        sb.AppendLine(consoleTextFormatter.FormatAsBold("Version Information"))
            .AppendLine($"\tAssembly location: {assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine(
                $"\tBuild time: {buildTime.ToString("yyyy-MM-dd HH:mm:ss")}{TimeAgoMessage(buildTime)}"
            );

        AddGitInformation(sb);
        AddPackageReferenceInformation(sb);

        sb.AppendLine();

        /*
         * Build information
         */

        sb.AppendLine(consoleTextFormatter.FormatAsBold("Build Information"));

        foreach (var buildArgument in _buildArguments)
        {
            var buildArgumentValue = assemblyInformationProvider.GetMetadata(buildArgument);
            sb.AppendLine($"\t{buildArgument}: {buildArgumentValue ?? "Unknown"}");
        }

        sb.AppendLine();

        /*
         * Process, Framework and OS
         */

        sb.AppendLine(consoleTextFormatter.FormatAsBold("Process, Framework and OS"))
            .AppendLine($"\tDEBUG flag: {assemblyInformationProvider.DebugFlag()}")
            .AppendLine($"\tProcess architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine($"\tFramework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"\tOS description: {RuntimeInformation.OSDescription}")
            .AppendLine($"\tOS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine();

        /*
         * Contact
         */

        sb.AppendLine(consoleTextFormatter.FormatAsBold("Contact"))
            .AppendLine(
                $"\tProject author: {constantsProvider.ProjectAuthor} "
                    + $"({constantsProvider.ProjectAuthorContactDetails})"
            )
            .AppendLine($"\tProject website: {constantsProvider.ProjectWebsite}")
            .AppendLine();
    }

    private void AddGitInformation(StringBuilder sb, bool underlineTopBranch = true)
    {
        sb.AppendLine($"\tGit description: {assemblyInformationProvider.GitDescribe()}");

        var gitBranches = assemblyInformationProvider.GitBranches();
        var gitTags = assemblyInformationProvider.GitTags();

        var gitRefs = gitBranches.Concat(gitTags.Select(x => $"{x} (Tag)")).ToArray();

        PrintList(sb, "Git refs", gitRefs, underlineTopBranch);

        var gitWorktreeChanges = assemblyInformationProvider.GitWorktreeChanges();

        PrintList(sb, "Git worktree changes", gitWorktreeChanges);
    }

    private void AddPackageReferenceInformation(StringBuilder sb)
    {
        var packageReferences = assemblyInformationProvider.GetPackageReferences();

        var packageReferencesAsStrings = packageReferences
            .Select(x => $"{x.Identity} | {x.Version}")
            .ToArray();

        PrintList(sb, "Package references", packageReferencesAsStrings);
    }

    private void PrintList(
        StringBuilder sb,
        string name,
        string[] set,
        bool underlineTopItem = false
    )
    {
        switch (set.Length)
        {
            case 0:
            {
                var none = "[NONE]";
                var item = underlineTopItem ? consoleTextFormatter.FormatAsUnderlined(none) : none;

                sb.AppendLine($"\t{name}: {item}");
                break;
            }
            case 1:
            {
                var item = underlineTopItem
                    ? consoleTextFormatter.FormatAsUnderlined(set[0])
                    : set[0];

                sb.AppendLine($"\t{name}: {item}");
                break;
            }

            default:
            {
                sb.AppendLine($"\t{name}:");

                var i = 0;

                foreach (var item in set)
                {
                    var itemToPrint =
                        i++ == 0 && underlineTopItem
                            ? consoleTextFormatter.FormatAsUnderlined(item)
                            : item;

                    sb.AppendLine($"\t\t{itemToPrint}");
                }

                break;
            }
        }
    }

    private string TimeAgoMessage(DateTime buildTime)
    {
        var message = InnerTimeAgoMessage(buildTime);

        return (message is null) ? "" : $" ({message})";
    }

    private record struct UnitAndMultiplier(string Unit, double Divisor, double LimitInMilliseconds);

    private static IEnumerable<UnitAndMultiplier> CreateUnits()
    {
        const double millisecond = 1.0;
        const double second = 1000.0;
        const double minute = 60.0 * second;
        const double hour = 60.0 * minute;
        const double day = 24.0 * hour;
        const double week = 7.0 * day;
        const double year = 365.0 * day;
        
        yield return new UnitAndMultiplier("millisecond", millisecond, second);
        yield return new UnitAndMultiplier("second", second, minute);
        yield return new UnitAndMultiplier("minute", minute, hour);
        yield return new UnitAndMultiplier("hour", hour, day);
        yield return new UnitAndMultiplier("day", day, week);
        yield return new UnitAndMultiplier("week", week, year);
        yield return new UnitAndMultiplier("year", year, double.MaxValue);
    }

    private string? InnerTimeAgoMessage(DateTime buildTime)
    {
        var unitsAndMultipliers = UnitsAndMultipliers.Value;

        var currentTime = dateAccessor.GetNow();
        var time = new TimeSpan(currentTime.Ticks - buildTime.Ticks).TotalMilliseconds;

        foreach (var unitAndMultiplier in unitsAndMultipliers)
        {
            if (time >= unitAndMultiplier.LimitInMilliseconds) continue;
            
            var scaled = time / unitAndMultiplier.Divisor;
            return $"{FloorAndPluralise(scaled, unitAndMultiplier.Unit)} ago";
        }
        
        //Should not occur because last condition should always succeed (double.MaxValue)
        //For compiler
        throw new Exception();
    }

    private static string FloorAndPluralise(double val, string unit)
    {
        var floored = Math.Floor(val);
        return floored <= 1.0 ? $"{floored} {unit}" : $"{floored} {unit}s";
    }
}
