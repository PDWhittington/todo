using System;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitPushCommandExecutor(IOutputWriter outputWriter)
    : GitCommandExecutorBase<GitPushCommand, VoidResult>(outputWriter),
        IGitPushCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, GitPushCommand command)
    {
        try
        {
            var currentBranch = command.BranchLocator.GetBranchForRepository(
                gitInterface.Repository
            );

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Pushing branch {currentBranch.FriendlyName} to origin"
            );

            gitInterface.Repository.Network.Push(currentBranch);

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

