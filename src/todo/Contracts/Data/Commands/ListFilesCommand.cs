using System;

namespace Todo.Contracts.Data.Commands;

public class ListFilesCommand : CommandBase
{
    [Flags]
    public enum FileLocationEnum
    {
        MainFolder = 1,
        ArchiveFolder = 2
    }

    [Flags]
    public enum FileTypeEnum
    {
        DayList = 1,
        TopicList = 2
    }

    public FileLocationEnum FileLocation { get; }

    public FileTypeEnum FileType { get; }

    private ListFilesCommand(FileLocationEnum fileLocation, FileTypeEnum fileType)
    {
        FileLocation = fileLocation;
        FileType = fileType;
    }

    public static ListFilesCommand Of(FileLocationEnum fileLocation, FileTypeEnum fileType)
        => new(fileLocation, fileType);
}
