using System.IO;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class OutputFolderPathProvider(IConfigurationProvider configurationProvider)
    : IOutputFolderPathProvider
{
    public string GetRootedOutputFolder()
    {
        var settingsPath = configurationProvider.ConfigInfo.Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(
            settingsFolder!,
            configurationProvider.ConfigInfo.Configuration.OutputFolder
        );
        return Path.GetFullPath(rootedPath);
    }

    public string GetRootedArchiveFolder()
    {
        var settingsPath = configurationProvider.ConfigInfo.Path;
        var settingsFolder = Path.GetDirectoryName(settingsPath);
        var rootedPath = Path.Combine(
            settingsFolder!,
            configurationProvider.ConfigInfo.Configuration.ArchiveFolderName
        );
        return Path.GetFullPath(rootedPath);
    }
}