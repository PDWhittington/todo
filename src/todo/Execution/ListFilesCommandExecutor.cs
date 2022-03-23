using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandExecutor : CommandExecutorBase<ListFilesCommand>, IListFilesCommandExecutor
{
    private readonly IFileListCreator _fileListCreator;

    public ListFilesCommandExecutor(IFileListCreator fileListCreator,
        IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _fileListCreator = fileListCreator;
    }

    public override void Execute(ListFilesCommand command)
    {
        var filesListText = GenerateText(command);

        OutputWriter.WriteLine(filesListText);
    }

    private string GenerateText(ListFilesCommand command)
    {
        var sb = new StringBuilder()
            .AppendLine(GetHeader(command))
            .AppendLine();

        var pathGroups = _fileListCreator
            .GetFiles(command.OutputFolder, command.ListFileType)
            .GroupBy(filePathInfo => new { filePathInfo.FolderType, filePathInfo.FileType })
            .ToArray();

        //Exit with confirmation of no files if none are returned.
        if (pathGroups.Length == 0)
        {
            sb.AppendLine("There are no files satisfying these criteria.");
            return sb.ToString();
        }

        for (var i = 0; i < pathGroups.Length; i++)
        {
            var pathGroup = pathGroups[i];

            sb.AppendLine($"{MapToFolderName(pathGroup.Key.FolderType)}, {MapToFileTypeName(pathGroup.Key.FileType)}");
            sb.AppendLine();

            foreach (var pathInfo in pathGroup)
            {
                sb.AppendLine($"\t{pathInfo.Path}");
            }

            if (i != pathGroups.Length - 1) sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string MapToFileTypeName(FileTypeEnum fileType) =>
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        fileType switch
        {
            FileTypeEnum.MarkdownDayList => "day lists",
            FileTypeEnum.MarkdownTopicList => "topic lists",
            _ => throw new Exception() //should be impossible
        };

    private static string MapToFolderName(FolderEnum folder) =>
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        folder switch
        {
            FolderEnum.TodoRoot => "Root folder",
            FolderEnum.Archive => "Archive folder",
            _ => throw new Exception() //should be impossible
        };

    private static string GetHeader(ListFilesCommand command)
    {
        string FileTypeMessage(ListFileTypeEnum listFileType)
            => listFileType switch
            {
                ListFileTypeEnum.DayList => "day todo lists",
                ListFileTypeEnum.TopicList => "topic todo lists",
                ListFileTypeEnum.DayList |
                    ListFileTypeEnum.TopicList => "both types of todo lists",
                _ => throw new Exception()
            };

        string FileLocationMessage(OutputFolderEnum outputFolder)
            => outputFolder switch
            {
                OutputFolderEnum.MainFolder => "the main folder",
                OutputFolderEnum.ArchiveFolder => "the archive folder",
                OutputFolderEnum.MainFolder |
                    OutputFolderEnum.ArchiveFolder => "both folders",
                _ => throw new Exception()
            };

        return $"Listing {FileTypeMessage(command.ListFileType)} in {FileLocationMessage(command.OutputFolder)}:-";
    }
}
