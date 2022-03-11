using System;
using System.Reflection;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class AssemblyInformationProvider : IAssemblyInformationProvider
{
    private readonly IConstantsProvider _constantsProvider;
    private readonly IManifestStreamProvider _manifestStreamProvider;

    public AssemblyInformationProvider(IConstantsProvider constantsProvider, IManifestStreamProvider manifestStreamProvider)
    {
        _constantsProvider = constantsProvider;
        _manifestStreamProvider = manifestStreamProvider;
    }

    public string GetCommitHash() =>
        _manifestStreamProvider.GetStringFromManifest(_constantsProvider.CommitHash.FullName).Trim();

    public DateTime GetBuildTime()
    {
        var dteStr = _manifestStreamProvider
            .GetStringFromManifest(_constantsProvider.BuildTime.FullName)
            .Trim();

        return DateTime.Parse(dteStr);
    }

    public string AssemblyLocation() => Assembly.GetCallingAssembly().Location;

    #if DEBUG
    public bool DebugFlag() => true;

    #else

    public bool DebugFlag() => false;

    #endif
}
