using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Commands;

public record ListFilesCommand : CommandBase
{
    public OutputFolderEnum OutputFolder { get; }

    public ListFileTypeEnum ListFileType { get; }

    public bool Bare { get; }

    private ListFilesCommand(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType, bool bare)
    {
        OutputFolder = outputFolder;
        ListFileType = listFileType;
        Bare = bare;
    }

    public static ListFilesCommand Of(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType,
        bool bare = false)
        => new(outputFolder, listFileType, bare);
}