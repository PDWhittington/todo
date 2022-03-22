using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem;

public class FolderCreator : IFolderCreator
{
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    public FolderCreator(IOutputFolderPathProvider outputFolderPathProvider)
    {
        _outputFolderPathProvider = outputFolderPathProvider;
    }

    public void CreateOutputFolder()
        => CreateIfDoesntExist(_outputFolderPathProvider.GetRootedOutputFolder());

    public void CreateArchiveFolder()
        => CreateIfDoesntExist(_outputFolderPathProvider.GetRootedArchiveFolder());

    public void CreateIfDoesntExist(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void CreateFromPathIfDoesntExist(string path)
    {
        var dir = Path.GetDirectoryName(path)
            ?? throw new ArgumentException("Directory cannot be parsed from path");

        CreateIfDoesntExist(dir);
    }
}
