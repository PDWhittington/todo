using System;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitAddCommand(string Path) : IGitCommand<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global

    public VoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine($"Staging {Path}");

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, Path);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
