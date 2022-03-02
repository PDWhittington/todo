using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Reporting;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitResetCommand : GitCommandBase<VoidResult>
{
    public bool Hard { get; }

    public GitResetCommand(bool hard = false)
    {
        Hard = hard;
    }

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            repo.Reset(Hard ? ResetMode.Hard : ResetMode.Soft);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }

    }
}
