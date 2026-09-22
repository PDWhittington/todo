using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class OutputFolderPathProvider(
    IConfigurationProvider configurationProvider,
    IFileSystemFactory fileSystemFactory)
    : IOutputFolderPathProvider
{
    private readonly IFileSystem _fileSystem = fileSystemFactory.Create();

    public string GetRootedOutputFolder()
    {
        var settingsPath = configurationProvider.ConfigInfo.Path;
        var settingsFolder = _fileSystem.GetDirectoryName(settingsPath);
        var rootedPath = _fileSystem.Combine(
            settingsFolder!,
            configurationProvider.ConfigInfo.Configuration.OutputFolder
        );
        return _fileSystem.GetFullPath(rootedPath);
    }

    public string GetRootedArchiveFolder()
    {
        var settingsPath = configurationProvider.ConfigInfo.Path;
        var settingsFolder = _fileSystem.GetDirectoryName(settingsPath);
        var rootedPath = _fileSystem.Combine(
            settingsFolder!,
            configurationProvider.ConfigInfo.Configuration.ArchiveFolderName
        );
        return _fileSystem.GetFullPath(rootedPath);
    }
}