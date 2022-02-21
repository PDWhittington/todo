using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandExecutor : CommandExecutorBase<ListFilesCommand>, IListFilesCommandExecutor
{
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IPathRootingProvider _pathRootingProvider;

    public ListFilesCommandExecutor(IDateListPathResolver dateListPathResolver, IPathRootingProvider pathRootingProvider)
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

        Console.WriteLine(fileList);
    }
}
