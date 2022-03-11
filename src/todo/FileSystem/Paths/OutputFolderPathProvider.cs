using System.IO;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class OutputFolderPathProvider : IOutputFolderPathProvider
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ISettingsPathProvider _settingsPathProvider;

    public OutputFolderPathProvider(IConfigurationProvider configurationProvider,
        ISettingsPathProvider settingsPathProvider)
    {
        _configurationProvider = configurationProvider;
        _settingsPathProvider = settingsPathProvider;
    }

    public string GetRootedOutputFolder()
    {
        var settingsPath = _settingsPathProvider.GetSettingsPathInHierarchy().Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(settingsFolder!, _configurationProvider.Config.OutputFolder);
        return Path.GetFullPath(rootedPath);
    }

    public string GetRootedArchiveFolder()
    {
        var settingsPath = _settingsPathProvider.GetSettingsPathInHierarchy().Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(settingsFolder!, _configurationProvider.Config.ArchiveFolderName);
        return Path.GetFullPath(rootedPath);
    }
}
