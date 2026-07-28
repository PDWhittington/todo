using System;
using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using CommitCommand = Todo.Contracts.Data.Commands.CommitCommand;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandExecutor(
    IDateAccessor dateAccessor,
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    IOutputFolderPathProvider outputFolderPathProvider,
    IOutputWriter outputWriter,
    ILogger<CommitCommandExecutor> logger
) : CommandExecutorBase<CommitCommand>(outputWriter, logger), ICommitCommandExecutor
{
    public override void Execute(CommitCommand commitCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received commit with override message '{commitMessage}'.",
            GetType(),
            nameof(Execute),
            commitCommand.CommitMessage
        );

        if (!configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Git is not enabled, and so a commit command does not make sense. ",
                GetType(),
                nameof(Execute)
            );

            throw new Exception(
                "Syncing does not make sense when UseGit is set to false in the settings file."
            );
        }

        var commitMessage =
            commitCommand.CommitMessage
            ?? $"Synced as at {dateAccessor.GetNow():yyyy-MM-dd HH:mm:ss}";

        Logger.LogInformation(
            "In {GetType}.{MethodName}: Final commit message: '{commitMessage}'. ",
            GetType(),
            nameof(Execute),
            commitMessage
        );

        OutputWriter.WriteLine("Committing todo files.");

        gitInterface.RunGitCommand<GitResetCommand, VoidResult>(new GitResetCommand());
        gitInterface.RunGitCommand<GitAddCommand, VoidResult>(
            new GitAddCommand(outputFolderPathProvider.GetRootedOutputFolder())
        );

        //Archive may not be nested within the OutputFolder
        gitInterface.RunGitCommand<GitAddCommand, VoidResult>(
            new GitAddCommand(outputFolderPathProvider.GetRootedArchiveFolder())
        );

        var commitResult = gitInterface.RunGitCommand<GitCommitCommand, CommitResult>(
            new GitCommitCommand(commitMessage)
        );

        if (commitResult.Exception is EmptyCommitException)
        {
            OutputWriter.WriteLine("No commit written -- was empty");
        }
    }
}