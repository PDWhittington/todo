using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public abstract record GitCommandBase<T>
{
    internal abstract T ExecuteCommand(IGitInterface gitInterface);
}
