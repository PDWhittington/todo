using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandExecutor : CommandExecutorBase<KillHtmlCommand>, IKillHtmlCommandExecutor
{
    private readonly IPathResolver _pathResolver;
    private readonly IFileDeleter _fileDeleter;

    public KillHtmlCommandExecutor(IPathResolver pathResolver, IFileDeleter fileDeleter)
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
