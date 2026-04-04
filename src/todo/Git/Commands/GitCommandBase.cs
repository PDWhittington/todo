using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public abstract class GitCommandBase<T>
{
    internal abstract T ExecuteCommand(IGitInterface gitInterface);
}
