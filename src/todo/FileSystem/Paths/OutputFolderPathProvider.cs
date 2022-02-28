using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class OutputFolderPathProvider : IOutputFolderPathProvider
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;

    public OutputFolderPathProvider(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    public string GetRootedOutputFolder()
        => _pathHelper.GetRootedToWorkingFolder(_configurationProvider.Config.OutputFolder);

    public string GetRootedArchiveFolder()
        => _pathHelper.GetRootedToWorkingFolder(_configurationProvider.Config.ArchiveFolderName);
}
