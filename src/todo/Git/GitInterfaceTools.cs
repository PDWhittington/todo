using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git;

public class GitInterfaceTools : IGitInterfaceTools
{
    public IFolderCreator FolderCreator { get; }

    public IOutputWriter OutputWriter { get; }

    public GitInterfaceTools(IFolderCreator folderCreator, IOutputWriter outputWriter)
    {
        FolderCreator = folderCreator;
        OutputWriter = outputWriter;
    }
}
