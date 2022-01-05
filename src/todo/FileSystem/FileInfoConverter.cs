using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class FileInfoConverter : IFileInfoConverter
{
    public FilePathInfo Convert(FilePathInfo filePathInfo, FileTypeEnum fileTypeEnum)
    {
        // var newExtension = fileTypeEnum switch
        // {
        //     FileTypeEnum.Html => "html",
        //     FileTypeEnum.Markdown => "md",
        // };
        //
        // var newPath = Path.ChangeExtension(filePathInfo.Path, "");

        throw new System.NotImplementedException();
    }
}
