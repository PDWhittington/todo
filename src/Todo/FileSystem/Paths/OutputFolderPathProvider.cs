using System.IO;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class OutputFolderPathProvider : IOutputFolderPathProvider
{
    private readonly IConfigurationProvider _configurationProvider;

    public OutputFolderPathProvider(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string GetRootedOutputFolder()
    {
        var settingsPath = _configurationProvider.ConfigInfo.Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(settingsFolder!,
            _configurationProvider.ConfigInfo.Configuration.OutputFolder);
        return Path.GetFullPath(rootedPath);
    }

    public string GetRootedArchiveFolder()
    {
        var settingsPath = _configurationProvider.ConfigInfo.Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(settingsFolder!,
            _configurationProvider.ConfigInfo.Configuration.ArchiveFolderName);
        return Path.GetFullPath(rootedPath);
    }
}
