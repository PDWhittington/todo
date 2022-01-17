using System;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class ContentFilePathResolver : IContentFilePathResolver
{
    private readonly IPathResolver _pathResolver;

    public ContentFilePathResolver(IPathResolver pathResolver)
    {
        _pathResolver = pathResolver;
    }

    public FilePathInfo GetPathFor(DateOnly date, FileTypeEnum fileType, bool allowNotPresent)
    {
        var pathInTodoFolder = _pathResolver.GetFilePath(date, fileType);
        var pathInArchiveFolder  = _pathResolver.GetArchiveFilePath(date, fileType);

        switch (File.Exists(pathInTodoFolder.Path), File.Exists(pathInArchiveFolder.Path), allowNotPresent)
        {
            case (true, false, _):
            case (false, false, true):
                return pathInTodoFolder; //Exists in todo root or in neither

            case (false, false, false):
                throw new Exception("File not found in either todo root folder or the archive folder");

            case (false, true, _):
                return pathInArchiveFolder; //Exists in archive but not in todo root

            case (true, true, _):
                throw new Exception("File found in both the todo root folder and the archive folder"); //Exists in both
        }
    }
}
