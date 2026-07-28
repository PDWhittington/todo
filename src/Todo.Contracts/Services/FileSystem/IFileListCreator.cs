using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IFileListCreator
{
    IEnumerable<T> GetFiles<T>(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType)
        where T : FilePathInfo;
}