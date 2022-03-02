using LibGit2Sharp;
using Todo.Contracts.Services.Reporting;

namespace Todo.Git.Commands;

public abstract class GitCommandBase<T>
{
    internal abstract T ExecuteCommand(IRepository repo, IOutputWriter? outputWriter);
}
