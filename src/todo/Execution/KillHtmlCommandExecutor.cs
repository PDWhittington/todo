using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandExecutor : CommandExecutorBase<KillHtmlCommand>, IKillHtmlCommandExecutor
{
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IFileDeleter _fileDeleter;

    public KillHtmlCommandExecutor(IDateListPathResolver dateListPathResolver, IFileDeleter fileDeleter)
    {
        _dateListPathResolver = dateListPathResolver;
        _fileDeleter = fileDeleter;
    }

    public override void Execute(KillHtmlCommand command)
    {
        _fileDeleter.Delete(_dateListPathResolver.GetOutputFolder(), "*.html");
        _fileDeleter.Delete(_dateListPathResolver.GetArchiveFolder(), "*.html");
    }
}
