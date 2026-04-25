using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.UI;

namespace Todo.Contracts.Services.Git;

public interface IGitInterfaceTools
{
    public IFolderCreator FolderCreator { get; }

    public IOutputWriter OutputWriter { get; }
}
