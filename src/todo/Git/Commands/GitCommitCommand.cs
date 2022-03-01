using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public class GitCommitCommand : GitSingleCommandBase
{
    public string Message { get; }

    public GitCommitCommand(string message)
    {
        Message = message;
    }

    protected override string SingleCommand() => $"commit -m \"{Message}\"";
}
