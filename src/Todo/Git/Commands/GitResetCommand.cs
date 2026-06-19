using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitResetCommand : IGitCommand<GitVoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public bool Hard { get; }

    public GitResetCommand(bool hard = false)
    {
        Hard = hard;
    }

    public GitVoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.Repository.Reset(Hard ? ResetMode.Hard : ResetMode.Soft);
            return new GitVoidResult(true, null);
        }
        catch (Exception e)
        {
            return new GitVoidResult(false, e);
        }

    }
}
