using System;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class BoilerPlateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IDateAccessor dateAccessor,
    IConstantsProvider constantsProvider
) : IBoilerPlateProvider
{
    public string GetBoilerPlate()
    {
        var sb = new StringBuilder();
        MakeBoilerPlate(sb);
        return sb.ToString();
    }

    public void MakeBoilerPlate(StringBuilder sb)
    {
        sb.AppendLine($"Assembly location: {assemblyInformationProvider.AssemblyLocation()}")
            .AppendLine($"Todo version (commit): {assemblyInformationProvider.GetCommitHash()}")
            .AppendLine(
                $"Build time: {assemblyInformationProvider.GetBuildTime().ToString("yyyy-MM-dd HH:mm:ss")}{TimeAgoMessage()}"
            )
            .AppendLine($"DEBUG flag: {assemblyInformationProvider.DebugFlag()}")
            .AppendLine($"Process architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine()
            .AppendLine($"Framework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"OS description: {RuntimeInformation.OSDescription}")
            .AppendLine($"OS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine()
            .AppendLine(
                $"Project author: {constantsProvider.ProjectAuthor} "
                    + $"({constantsProvider.ProjectAuthorContactDetails})"
            )
            .AppendLine($"Project website: {constantsProvider.ProjectWebsite}")
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
