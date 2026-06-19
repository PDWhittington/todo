using System;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitAddCommand(string Path) : IGitCommand<GitVoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global

    public GitVoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine($"Staging {Path}");

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, Path);
            return new GitVoidResult(true, null);
        }
        catch (Exception e)
        {
            return new GitVoidResult(false, e);
        }
    }
}
