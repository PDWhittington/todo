using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowAllInMainCommandExecutor(
    ITextFileLauncher fileOpener,
    IFileListCreator fileListCreator,
    IOutputWriter outputWriter,
    ILogger<ShowAllInMainCommandExecutor> logger)
    : CommandExecutorBase<ShowAllInMainCommand>(outputWriter, logger), IShowAllInMainCommandExecutor
{
    public override void Execute(ShowAllInMainCommand command)
    {
        Logger.LogInformation("Entered {GetType}.{MethodName}", GetType(), nameof(Execute));

        var files = fileListCreator
            .GetFiles<DayListFilePathInfo>(OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList)
            .OrderBy(file => file.Date)
            .ThenBy(file => file.Path, StringComparer.Ordinal)
            .ToArray();
        
        if (files.Length == 0)
        {
            OutputWriter.WriteLine("There are no day todo lists in the main folder.");
            return;
        }

        foreach (var file in files)
        {
            fileOpener.LaunchFiles(file.Path);
        }
    }
}
