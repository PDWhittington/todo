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
    IConsoleTextFormatter consoleTextFormatter) : IBoilerPlateProvider
{
    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    public void MakeBoilerPlate(StringBuilder sb)
    {
        sb.AppendLine(consoleTextFormatter.FormatAsUnderlined("Version Information"))
            .AppendLine($"\tAssembly location: {assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine($"\tTodo version (commit): {assemblyInformationProvider.GetCommitHash()}")
            .AppendLine(
                $"\tBuild time: {assemblyInformationProvider.GetBuildTime().ToString("yyyy-MM-dd HH:mm:ss")}{TimeAgoMessage()}"
            )
            .AppendLine()
            .AppendLine(consoleTextFormatter.FormatAsUnderlined("Build Information"))
            .AppendBuildInformation()
            .AppendLine()
            .AppendLine(consoleTextFormatter.FormatAsUnderlined("Process, Framework and OS"))
            .AppendLine($"\tDEBUG flag: {assemblyInformationProvider.DebugFlag()}")
            .AppendLine($"\tProcess architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine($"\tFramework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"\tOS description: {RuntimeInformation.OSDescription}")
            .AppendLine($"\tOS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine()
            .AppendLine(consoleTextFormatter.FormatAsUnderlined("Contact"))
            .AppendLine(
                $"\tProject author: {constantsProvider.ProjectAuthor} "
                    + $"({constantsProvider.ProjectAuthorContactDetails})")
            .AppendLine($"\tProject website: {constantsProvider.ProjectWebsite}")
            .AppendLine();
    }

    private string TimeAgoMessage()
    {
        var message = InnerTimeAgoMessage();

        return (message is null) ? "" : $" ({message})";
    }

    private string? InnerTimeAgoMessage()
    {
        var buildTime = assemblyInformationProvider.GetBuildTime();

        var currentTime = dateAccessor.GetNow();

        var time = new TimeSpan(currentTime.Ticks - buildTime.Ticks).TotalMilliseconds;

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

        return (floored == 1.0) ? $"{floored} {unit}" : $"{floored} {unit}s";
    }
}

static class BuildInfoExtensions
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
        "TieredCompilation"
    ];
    
    public static StringBuilder AppendBuildInformation(this StringBuilder sb)
    {
        var asm = Assembly.GetExecutingAssembly();
        
        foreach (var arg in _buildArguments)
        {
            sb.AppendLine($"\t{arg}: {GetMetadata(asm, arg) ?? "Unknown"}");
        }
        
        return sb;
    }
    
    private static string? GetMetadata(Assembly asm, string key)
    {
        return asm.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == key)?.Value;
    }
}