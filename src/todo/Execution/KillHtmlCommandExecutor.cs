using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandExecutor : CommandExecutorBase<KillHtmlCommand>, IKillHtmlCommandExecutor
{
    private readonly IPathRootingProvider _pathRootingProvider;
    private readonly IFileDeleter _fileDeleter;

    public KillHtmlCommandExecutor(IPathRootingProvider pathRootingProvider, IFileDeleter fileDeleter)
    {
        _pathRootingProvider = pathRootingProvider;
        _fileDeleter = fileDeleter;
    }

    public override void Execute(KillHtmlCommand command)
    {
        _fileDeleter.Delete(_pathRootingProvider.GetRootedOutputFolder(), "*.html");
        _fileDeleter.Delete(_pathRootingProvider.GetRootedArchiveFolder(), "*.html");
    }
}
