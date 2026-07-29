using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnarchiveCommandExecutor(
    IDateListPathResolver dateListPathResolver,
    IConfigurationProvider configurationProvider,
    IFileMover fileMover,
    IFolderCreator folderCreator,
    IGitInterface gitInterface,
    IOutputWriter outputWriter,
    ILogger<UnarchiveCommandExecutor> logger)
    : FileMoveExecutorBase<UnarchiveCommand>(configurationProvider, gitInterface, 
        fileMover, folderCreator, outputWriter, logger), 
    IUnarchiveCommandExector
{
    protected override FilePathInfo GetSourcePath(UnarchiveCommand command) =>
        dateListPathResolver.GetArchiveFilePathFor(
            command.DateOfFileToArchive,
            FileTypeEnum.MarkdownDayList);

    protected override FilePathInfo GetDestinationPath(UnarchiveCommand command) =>
        dateListPathResolver.GetFilePathFor(
            command.DateOfFileToArchive,
            FileTypeEnum.MarkdownDayList);
}
