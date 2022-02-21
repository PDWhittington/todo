using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class PathRootingProvider : IPathRootingProvider
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;

    public PathRootingProvider(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    public string GetRootedOutputFolder()
        => _pathHelper.GetRootedToWorkingFolder(_configurationProvider.Config.OutputFolder);

    public string GetRootedArchiveFolder()
        => _pathHelper.GetRootedToWorkingFolder(_configurationProvider.Config.ArchiveFolderName);
}
