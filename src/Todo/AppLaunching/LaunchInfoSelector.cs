using System.Runtime.InteropServices;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.AppLaunching;

public class LaunchInfoSelector(IEnvironmentVariableProvider environmentVariableProvider)
    : ILaunchInfoSelector
{
    public ProcessLaunchInfo SelectLaunchInfoForThisOs(PerOsLaunchInfos perOsLaunchInfos)
    {
        if (TryGetOverride(perOsLaunchInfos, out var overrideLaunchInfo))
        {
            return overrideLaunchInfo!;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return perOsLaunchInfos.Windows;
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? perOsLaunchInfos.OSX
            : perOsLaunchInfos.Linux; // Assume that any unrecognised OS is a POSIX variant.
    }

    private bool TryGetOverride(PerOsLaunchInfos perOsLaunchInfos, out ProcessLaunchInfo? value)
    {
        var pathVariableNameIsExists = !string.IsNullOrWhiteSpace(perOsLaunchInfos.EnvironmentVariableToOverridePath);
        var parameterVariableNameIsExists = !string.IsNullOrWhiteSpace(perOsLaunchInfos.EnvironmentVariableToOverrideArguments);

        if (!pathVariableNameIsExists || !parameterVariableNameIsExists)
        {
            value = null;
            return false;
        }

        var overridePathExists = environmentVariableProvider.TryGetEnvironmentVariable(
            perOsLaunchInfos.EnvironmentVariableToOverridePath, out var overridePath);

        var overrideArgumentsExists = environmentVariableProvider.TryGetEnvironmentVariable(
            perOsLaunchInfos.EnvironmentVariableToOverrideArguments, out var overrideParameters);

        if (!overridePathExists || !overrideArgumentsExists)
        {
            value = null;
            return false;
        }
        
        value = new ProcessLaunchInfo(overridePath!, overrideParameters!);
        return true;
    }
}
