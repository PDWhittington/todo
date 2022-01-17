using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

public class KillHtmlExecutor : CommandExecutorBase<KillHtmlCommand>, IKillHtmlExecutor
{
    private readonly IPathResolver _pathResolver;
    private readonly IFileDeleter _fileDeleter;

    public KillHtmlExecutor(IPathResolver pathResolver, IFileDeleter fileDeleter)
    {
        _pathResolver = pathResolver;
        _fileDeleter = fileDeleter;
    }

    public override void Execute(KillHtmlCommand command)
    {
        _fileDeleter.Delete(_pathResolver.GetOutputFolder(), "*.html");
        _fileDeleter.Delete(_pathResolver.GetArchiveFolder(), "*.html");
    }
}
