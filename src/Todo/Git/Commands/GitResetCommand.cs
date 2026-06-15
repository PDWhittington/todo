using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitResetCommand : IGitCommand<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public bool Hard { get; }

    public GitResetCommand(bool hard = false)
    {
        Hard = hard;
    }

    public VoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.Repository.Reset(Hard ? ResetMode.Hard : ResetMode.Soft);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }

    }
}
