namespace Todo.Contracts.Data.FileSystem;

public record FilePathInfo
{
    public string Path { get; }

    public FileTypeEnum FileType { get; }

    public FolderEnum FolderType { get; }
    
    public override string ToString() => Path;

    protected FilePathInfo(string path, FileTypeEnum fileType, 
        FolderEnum folderType)
    {
        Path = System.IO.Path.IsPathRooted(path) ? path
            : throw new Exception("Only rooted paths are valid");

        FileType = fileType;
        FolderType = folderType;
    }
    
    public static FilePathInfo Of(string path, FileTypeEnum fileType, 
        FolderEnum folderType)
        => new(path, fileType, folderType);
}
