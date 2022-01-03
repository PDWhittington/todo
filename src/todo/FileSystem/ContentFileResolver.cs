using System;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class ContentFileResolver : IContentFileResolver
{
    private readonly IFileNamer _fileNamer;

    public ContentFileResolver(IFileNamer fileNamer)
    {
        _fileNamer = fileNamer;
    }

    public FilePathInfo GetPathFor(DateOnly date, FileTypeEnum fileType)
    {
        var pathInTodoFolder = _fileNamer.GetFilePath(date, fileType);
        var pathInArchiveFolder  = _fileNamer.GetArchiveFilePath(date, fileType);

        switch (File.Exists(pathInTodoFolder.Path), File.Exists(pathInArchiveFolder.Path))
        {
            case (true, false):
            case (false, false): return pathInTodoFolder; //Exists in todo root or in neither
            case (false, true): return pathInArchiveFolder; //Exists in archive but not in todo root
            case (true, true): throw new Exception("File found in both the todo root folder and the archive folder"); //Exists in both
        }
    }
}
