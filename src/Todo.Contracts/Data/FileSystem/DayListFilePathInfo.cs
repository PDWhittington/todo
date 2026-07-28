namespace Todo.Contracts.Data.FileSystem;

public record DayListFilePathInfo : FilePathInfo
{
    public DateOnly Date { get; }

    private DayListFilePathInfo(string path, FileTypeEnum fileType,
        FolderEnum folderType, DateOnly date)
        : base(path, fileType, folderType)
    {
        Date = date;
    }
    
    public static DayListFilePathInfo Of(string path, FileTypeEnum fileType,
        FolderEnum folderType, DateOnly date)
        => new(path, fileType, folderType, date);
}