using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IFileListCreator
{
    FilePathInfo[] GetFiles(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType);
}
