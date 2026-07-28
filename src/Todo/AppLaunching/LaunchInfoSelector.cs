using System.Runtime.InteropServices;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.AppLaunching;

public class LaunchInfoSelector(IEnvironmentVariableProvider EnvironmentVariableProvider) : ILaunchInfoSelector
{
    public ProcessLaunchInfo SelectLaunchInfoForThisOS(PerOsLaunchInfos perOsLaunchInfos)
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
        var pathVariableNameIsEmpty = string.IsNullOrWhiteSpace(perOsLaunchInfos.EnvironmentVariableToOverridePath);
        var parameterVariableNameIsEmpty = string.IsNullOrWhiteSpace(perOsLaunchInfos.EnvironmentVariableToOverrideParameters);

        if (!pathVariableNameIsEmpty && !parameterVariableNameIsEmpty)
        {
            var overridePathExists = EnvironmentVariableProvider.TryGetEnvironmentVariable(
                perOsLaunchInfos.EnvironmentVariableToOverridePath, out var overridePath);
                
            var overrideParametersExists = EnvironmentVariableProvider.TryGetEnvironmentVariable(
                perOsLaunchInfos.EnvironmentVariableToOverrideParameters, out var overrideParameters);

            if (overridePathExists && overrideParametersExists)
            {
                value = new ProcessLaunchInfo(overridePath!, overrideParameters!);
                return true;                
            }
        }
        
        value = null;
        return false;
    }
}