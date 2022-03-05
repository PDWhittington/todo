using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandExecutor : CommandExecutorBase<ListFilesCommand>, IListFilesCommandExecutor
{
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IOutputFolderPathProvider _pathRootingProvider;

    public ListFilesCommandExecutor(IDateListPathResolver dateListPathResolver,
        IOutputFolderPathProvider pathRootingProvider, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _dateListPathResolver = dateListPathResolver;
        _pathRootingProvider = pathRootingProvider;
    }

    public override void Execute(ListFilesCommand command)
    {
        var pattern = _dateListPathResolver.GetRegExForThisFileType();

        bool Filter(string path)
        {
            var fileName = Path.GetFileName(path);

            var match = Regex.Match(fileName, pattern, RegexOptions.None);

            var isDayList = match.Success;

            return isDayList && command.FileType.HasFlag(ListFilesCommand.FileTypeEnum.DayList) ||
                   !isDayList && command.FileType.HasFlag(ListFilesCommand.FileTypeEnum.TopicList);
        }

        var pathsInRelevantFolders = new[]
            {
                command.FileLocation.HasFlag(ListFilesCommand.FileLocationEnum.MainFolder)
                    ? Directory.GetFiles(_pathRootingProvider.GetRootedOutputFolder())
                    : Array.Empty<string>(),

                command.FileLocation.HasFlag(ListFilesCommand.FileLocationEnum.ArchiveFolder)
                    ? Directory.GetFiles(_pathRootingProvider.GetRootedArchiveFolder())
                    : Array.Empty<string>()

            }.SelectMany(x => x)
            .Where(Filter);

        var fileList = string.Join(Environment.NewLine, pathsInRelevantFolders);

        OutputWriter.WriteLine(GetMessage(command));
        OutputWriter.WriteLine(fileList);
    }

    static string GetMessage(ListFilesCommand command)
    {
        string FileTypeMessage(ListFilesCommand.FileTypeEnum fileType)
            => fileType switch
            {
                ListFilesCommand.FileTypeEnum.DayList => "day todo lists",
                ListFilesCommand.FileTypeEnum.TopicList => "topic todo lists",
                ListFilesCommand.FileTypeEnum.DayList |
                    ListFilesCommand.FileTypeEnum.TopicList => "both types of todo lists",
                _ => throw new Exception()
            };

        string FileLocationMessage(ListFilesCommand.FileLocationEnum fileLocation)
            => fileLocation switch
            {
                ListFilesCommand.FileLocationEnum.MainFolder => "the main folder",
                ListFilesCommand.FileLocationEnum.ArchiveFolder => "the archive folder",
                ListFilesCommand.FileLocationEnum.MainFolder |
                    ListFilesCommand.FileLocationEnum.ArchiveFolder => "both folders",
                _ => throw new Exception()
            };

        return $"Listing {FileTypeMessage(command.FileType)} in {FileLocationMessage(command.FileLocation)}:-";
    }
}
