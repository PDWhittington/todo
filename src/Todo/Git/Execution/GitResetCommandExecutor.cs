using System;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitResetCommandExecutor(IOutputWriter outputWriter, ILogger<GitResetCommandExecutor> logger)
    : GitCommandExecutorBase<GitResetCommand, VoidResult>(outputWriter, logger),
        IGitResetCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface,
        GitResetCommand gitResetCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName} (Hard: {hard}).",
            GetType(),
            nameof(RunGitCommand),
            gitResetCommand.GetType().Name,
            gitResetCommand.Hard);
        
        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Attempting LibGit2Sharp reset...",
                GetType(),
                nameof(RunGitCommand));
            
            gitInterface.Repository.Reset(gitResetCommand.Hard ? ResetMode.Hard : ResetMode.Soft);
            
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Reset done.",
                GetType(),
                nameof(RunGitCommand));

            return new VoidResult(true);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Git reset failed. Exception message: {exceptionMessage}...",
                GetType(),
                nameof(RunGitCommand),
                e.Message);
            
            return new VoidResult(false, e);
        }
    }
}