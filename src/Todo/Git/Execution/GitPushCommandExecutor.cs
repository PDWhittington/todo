using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitPushCommandExecutor(IOutputWriter outputWriter, ILogger<GitPushCommandExecutor> logger)
    : GitCommandExecutorBase<GitPushCommand, VoidResult>(outputWriter, logger),
        IGitPushCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, 
        GitPushCommand gitPushCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName} (BranchLocator: {branchLocator}).",
            GetType(),
            nameof(RunGitCommand),
            gitPushCommand.GetType().Name,
            gitPushCommand.BranchLocator);
        
        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Locating branch...",
                GetType(),
                nameof(RunGitCommand));
            
            var currentBranch = gitPushCommand
                .BranchLocator
                .GetBranchForRepository(gitInterface.Repository);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Branch located: {BranchName}",
                GetType(),
                nameof(RunGitCommand),
                currentBranch.FriendlyName);
            
            OutputWriter.WriteLine(
                $"Pushing branch {currentBranch.FriendlyName} to origin");

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Attempting LibGit2Sharp push: {BranchName}",
                GetType(),
                nameof(RunGitCommand),
                currentBranch.FriendlyName);
            
            gitInterface.Repository.Network.Push(currentBranch);

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Push failed. Exception message: {exceptionMessage}...",
                GetType(),
                nameof(RunGitCommand),
                e.Message);
            
            return new VoidResult(false, e);
        }
    }
}