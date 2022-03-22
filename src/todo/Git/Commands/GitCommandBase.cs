using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Commands;

public abstract class GitCommandBase<T>
{
    internal abstract T ExecuteCommand(IGitInterface gitInterface);
}
