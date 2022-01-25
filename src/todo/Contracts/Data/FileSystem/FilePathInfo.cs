namespace Todo.Contracts.Data.FileSystem;

public struct FilePathInfo
{
    public string Path { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FileTypeEnum FileType { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FolderEnum FolderType { get; }

    private FilePathInfo(string path, FileTypeEnum fileType, FolderEnum folderType)
    {
        Path = path;
        FileType = fileType;
        FolderType = folderType;
    }

    public static FilePathInfo Of(string path, FileTypeEnum fileType, FolderEnum folderType)
        => new(path, fileType, folderType);
}
