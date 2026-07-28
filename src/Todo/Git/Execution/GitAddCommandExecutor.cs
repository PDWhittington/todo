using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitAddCommandExecutor(
    IOutputWriter outputWriter,
    ILogger<GitAddCommandExecutor> logger
) : GitCommandExecutorBase<GitAddCommand, VoidResult>(outputWriter, logger), IGitAddCommandExecutor
{
    public override VoidResult RunGitCommand(
        IGitInterface gitInterface,
        GitAddCommand gitAddCommand
    )
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName} (Path: {path}).",
            GetType(),
            nameof(RunGitCommand),
            gitAddCommand.GetType().Name,
            gitAddCommand.Path
        );

        try
        {
            OutputWriter.WriteLine($"Staging {gitAddCommand.Path}");

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Starting LibGit2Sharp stage...",
                GetType(),
                nameof(RunGitCommand)
            );

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitAddCommand.Path); //Todo: currently failing

            Logger.LogInformation(
                "In {GetType}.{MethodName}: LibGit2Sharp stage finished...",
                GetType(),
                nameof(RunGitCommand)
            );

            return new VoidResult(true);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: LibGit2Sharp stage failed. Exception message: {exceptionMessage}...",
                GetType(),
                nameof(RunGitCommand),
                e.Message
            );

            return new VoidResult(false, e);
        }
    }
}
