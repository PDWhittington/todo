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

    private void CreateIfDoesntExist(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
