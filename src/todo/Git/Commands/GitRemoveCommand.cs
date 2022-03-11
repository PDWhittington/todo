using System;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitRemoveCommand : GitCommandBase<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string [] Paths { get; }

    public GitRemoveCommand(params string [] paths)
    {
        Paths = paths;
    }

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            LibGit2Sharp.Commands.Remove(repo, Paths);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
