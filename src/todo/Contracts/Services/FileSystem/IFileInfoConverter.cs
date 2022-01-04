using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IFileInfoConverter
{
    FilePathInfo Convert(FilePathInfo filePathInfo, FileTypeEnum fileTypeEnum);
}
