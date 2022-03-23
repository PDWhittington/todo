using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Commands;

public class ListFilesCommand : CommandBase
{
    public OutputFolderEnum OutputFolder { get; }

    public ListFileTypeEnum ListFileType { get; }

    private ListFilesCommand(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType)
    {
        OutputFolder = outputFolder;
        ListFileType = listFileType;
    }

    public static ListFilesCommand Of(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType)
        => new(outputFolder, listFileType);
}
