using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Reporting;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitPushCommand : GitCommandBase<VoidResult>
{
    public string BranchName { get; }

    public GitPushCommand() : this("HEAD") { }

    public GitPushCommand(string branchName)
    {
        BranchName = branchName;
    }

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            var currentBranch = repo.Branches[BranchName];

            outputWriter?.WriteLine($"Pushing branch {BranchName} to origin");
            repo.Network.Push(currentBranch);

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }

    }
}
