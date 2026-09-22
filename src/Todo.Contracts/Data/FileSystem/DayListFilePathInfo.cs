using Todo.Contracts.Services.FileSystem;

namespace Todo.Contracts.Data.FileSystem;

public record DayListFilePathInfo : FilePathInfo
{
    public DateOnly Date { get; }

    private DayListFilePathInfo(string path, FileTypeEnum fileType,
        FolderEnum folderType, DateOnly date, IFileSystem fileSystem)
        : base(path, fileType, folderType, fileSystem)
    {
        Date = date;
    }

    public static DayListFilePathInfo Of(string path, FileTypeEnum fileType,
        FolderEnum folderType, DateOnly date, IFileSystem fileSystem)
        => new(path, fileType, folderType, date, fileSystem);
}