using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandExecutor : CommandExecutorBase<ListFilesCommand>, IListFilesCommandExecutor
{
    private readonly IPathResolver _pathResolver;
    private readonly IConfigurationProvider _configurationProvider;

    public ListFilesCommandExecutor(IPathResolver pathResolver,
        IConfigurationProvider configurationProvider)
    {
        _pathResolver = pathResolver;
        _configurationProvider = configurationProvider;
    }

    public override void Execute(ListFilesCommand command)
    {
        var pattern = _pathResolver.GetRegExForDayListFormat();

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
                    ? Directory.GetFiles(_pathResolver.GetOutputFolder())
                    : new string[] { },

                command.FileLocation.HasFlag(ListFilesCommand.FileLocationEnum.ArchiveFolder)
                    ? Directory.GetFiles(_pathResolver.GetArchiveFolder())
                    : new string[] { }

            }.SelectMany(x => x)
            .Where(Filter);

        var fileList = string.Join(Environment.NewLine, pathsInRelevantFolders);

        Console.WriteLine(fileList);
    }
}
