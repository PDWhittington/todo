using System;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitAddCommand : GitCommandBase<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string Path { get; }

    public GitAddCommand(string path)
    {
        Path = path;
    }

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            outputWriter?.WriteLine($"Staging {Path}");
            LibGit2Sharp.Commands.Stage(repo, Path);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
