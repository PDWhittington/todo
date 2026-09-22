using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem;

public class FolderCreator(
    IOutputFolderPathProvider outputFolderPathProvider,
    IFileSystemFactory fileSystemFactory) : IFolderCreator
{
    private readonly IFileSystem _fileSystem = fileSystemFactory.Create();

    public void CreateOutputFolder() =>
        CreateIfDoesntExist(outputFolderPathProvider.GetRootedOutputFolder());

    public void CreateArchiveFolder() =>
        CreateIfDoesntExist(outputFolderPathProvider.GetRootedArchiveFolder());

    public void CreateIfDoesntExist(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void CreateFromPathIfDoesntExist(string path)
    {
        var dir =
            _fileSystem.GetDirectoryName(path)
            ?? throw new ArgumentException("Directory cannot be parsed from path");

        CreateIfDoesntExist(dir);
    }
}