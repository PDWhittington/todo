using System;
using System.IO;

namespace Todo.Contracts.Data.FileSystem;

public record FilePathInfo
{
    public string Path { get; }

    public FileTypeEnum FileType { get; }

    public FolderEnum FolderType { get; }
    
    public DateOnly? Date { get; }
    
    public override string ToString() => Path;

    private FilePathInfo(string path, FileTypeEnum fileType, 
        FolderEnum folderType, DateOnly? date)
    {
        Path = System.IO.Path.IsPathRooted(path) ? path
            : throw new Exception("Only rooted paths are valid");

        FileType = fileType;
        FolderType = folderType;
        Date = date;
    }

    // public static FilePathInfo Of(string path, FileTypeEnum fileType, FolderEnum folderType)
    //     => Of(path, fileType, folderType, null);
    
    public static FilePathInfo Of(string path, FileTypeEnum fileType, 
        FolderEnum folderType, DateOnly? date)
        => new(path, fileType, folderType, date);
}
