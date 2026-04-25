using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git;

public class GitInterfaceTools(IFolderCreator folderCreator, IOutputWriter outputWriter) : IGitInterfaceTools
{
    public IFolderCreator FolderCreator { get; } = folderCreator;

    public IOutputWriter OutputWriter { get; } = outputWriter;
}
