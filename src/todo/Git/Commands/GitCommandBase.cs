using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public abstract class GitCommandBase
{
    internal abstract bool ExecuteCommand(IGitInterface gitInterface);
}
