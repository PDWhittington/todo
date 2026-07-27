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
public class ArchiveCommandExecutor(
    IDateListPathResolver dateListPathResolver,
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    IOutputWriter outputWriter,
    IFolderCreator folderCreator,
    ILogger<ArchiveCommandExecutor> logger)
    : FileMoveExecutorBase<ArchiveCommand>(configurationProvider, gitInterface, outputWriter, folderCreator, logger),
        IArchiveCommandExecutor
{
    protected override FilePathInfo GetSourcePath(ArchiveCommand command)
        => dateListPathResolver.GetFilePathFor(command.DateOfFileToArchive, FileTypeEnum.MarkdownDayList);

    protected override FilePathInfo GetDestinationPath(ArchiveCommand command)
        => dateListPathResolver.GetArchiveFilePathFor(command.DateOfFileToArchive, FileTypeEnum.MarkdownDayList);
}
