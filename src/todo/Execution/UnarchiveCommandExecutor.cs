using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnarchiveCommandExecutor : FileMoveExecutorBase<UnarchiveCommand>
{
    private readonly IDateListPathResolver _dateListPathResolver;

    public UnarchiveCommandExecutor(IDateListPathResolver dateListPathResolver,
        IConfigurationProvider configurationProvider, IGitInterface gitInterface,
        IOutputWriter outputWriter, IFolderCreator folderCreator)
        : base(configurationProvider, gitInterface, outputWriter, folderCreator)
    {
        _dateListPathResolver = dateListPathResolver;
    }

    protected override FilePathInfo GetSourcePath(UnarchiveCommand command)
        => _dateListPathResolver.GetArchiveFilePathFor(command.DateOfFileToArchive, FileTypeEnum.Markdown);

    protected override FilePathInfo GetDestinationPath(UnarchiveCommand command)
        => _dateListPathResolver.GetFilePathFor(command.DateOfFileToArchive, FileTypeEnum.Markdown);
}
