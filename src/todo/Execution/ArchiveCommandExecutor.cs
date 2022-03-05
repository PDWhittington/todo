using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ArchiveCommandExecutor : FileMoveExecutorBase<ArchiveCommand>, IArchiveCommandExecutor
{
    private readonly IDateListPathResolver _dateListPathResolver;

    public ArchiveCommandExecutor(IDateListPathResolver dateListPathResolver,
        IConfigurationProvider configurationProvider, IGitInterface gitInterface,
        IOutputWriter outputWriter)
        : base(configurationProvider, gitInterface, outputWriter)
    {
        _dateListPathResolver = dateListPathResolver;
    }

    protected override FilePathInfo GetSourcePath(ArchiveCommand command)
        => _dateListPathResolver.GetFilePathFor(command.DateOfFileToArchive, FileTypeEnum.Markdown);

    protected override FilePathInfo GetDestinationPath(ArchiveCommand command)
        => _dateListPathResolver.GetArchiveFilePathFor(command.DateOfFileToArchive, FileTypeEnum.Markdown);
}
