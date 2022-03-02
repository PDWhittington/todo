using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class VersionProvider : IVersionProvider
{
    private readonly IConstantsProvider _constantsProvider;
    private readonly IManifestStreamProvider _manifestStreamProvider;

    public VersionProvider(IConstantsProvider constantsProvider, IManifestStreamProvider manifestStreamProvider)
    {
        _constantsProvider = constantsProvider;
        _manifestStreamProvider = manifestStreamProvider;
    }

    public string GetVersion() =>
        _manifestStreamProvider.GetStringFromManifest(_constantsProvider.CommitInfo.FullName).Trim();
}
