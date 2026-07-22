using System;
using System.Linq;
using System.Reflection;
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
    
    
    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    public void MakeBoilerPlate(StringBuilder sb)
    {
        /*
         * Version information
         */

        var buildTime = assemblyInformationProvider.GetBuildTime();

        sb.AppendLine(consoleTextFormatter.FormatAsBold("Version Information"))
            .AppendLine($"\tAssembly location: {assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine($"\tBuild time: {buildTime.ToString("yyyy-MM-dd HH:mm:ss")}{TimeAgoMessage(buildTime)}");

        AddGitInformation(sb);

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
                    + $"({constantsProvider.ProjectAuthorContactDetails})")
            .AppendLine($"\tProject website: {constantsProvider.ProjectWebsite}")
            .AppendLine();
    }

    private void AddGitInformation(StringBuilder sb)
    {
        sb.AppendLine($"\tGit description: {assemblyInformationProvider.GitDescribe()}");

        var gitBranches = assemblyInformationProvider.GitBranches();
        var gitTags = assemblyInformationProvider.GitTags();
        
        var gitRefs = gitBranches.Concat(gitTags.Select(x => $"{x} (Tag)")).ToArray();

        PrintList(sb, "Git refs", gitRefs, true);

        var gitWorktreeChanges = assemblyInformationProvider.GitWorktreeChanges();
        
        PrintList(sb, "Git worktree changes", gitWorktreeChanges);
    }

    private void PrintList(StringBuilder sb, string name, string[] set, bool underlineTopItem = false)
    {
        switch (set.Length)
        {
            case 0:
            {
                var none = "[NONE]";
                var item = underlineTopItem 
                    ? consoleTextFormatter.FormatAsUnderlined(none)
                    : none;
                
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
                    var itemToPrint = i++ == 0 && underlineTopItem
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

    private string? InnerTimeAgoMessage(DateTime buildTime)
    {
        var currentTime = dateAccessor.GetNow();

        var time = new TimeSpan(currentTime.Ticks - buildTime.Ticks).TotalMilliseconds;

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (time < 0.0)
            return null;

        if (time < 1000.0)
            return $"{FloorAndPluralise(time, "millisecond")}  ago";

        time /= 1000.0;

        if (time < 60.0)
            return $"{FloorAndPluralise(time, "second")} ago";

        time /= 60.0;

        if (time < 60.0)
            return $"{FloorAndPluralise(time, "minute")} ago";

        time /= 60.0;

        if (time < 60.0)
            return $"{FloorAndPluralise(time, "hour")} ago";

        time /= 24.0;

        if (time < 365.0)
            return $"{FloorAndPluralise(time, "day")} ago";

        time /= 365.0;

        return $"{FloorAndPluralise(time, "year")} ago";
    }

    private static string FloorAndPluralise(double val, string unit)
    {
        var floored = Math.Floor(val);

        // ReSharper disable once CompareOfFloatsByEqualityOperator -- fine because comparison is with a whole number
        return (floored == 1.0) ? $"{floored} {unit}" : $"{floored} {unit}s";
    }
}