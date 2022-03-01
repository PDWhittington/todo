using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public abstract class GitSingleCommandBase : GitCommandBase
{
    protected abstract string SingleCommand();

    internal override bool ExecuteCommand(IGitInterface gitInterface)
        => gitInterface.RunSpecialGitCommand(SingleCommand());
}
