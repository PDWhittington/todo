using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitCommitCommandExecutor(IOutputWriter outputWriter, ILogger<GitCommitCommandExecutor> logger)
    : GitCommandExecutorBase<GitCommitCommand, CommitResult>(outputWriter, logger),
        IGitCommitCommandExecutor
{
    public override CommitResult RunGitCommand(
        IGitInterface gitInterface, GitCommitCommand gitCommitCommand)
    {
        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Received {TypeName} with override message '{commitMessage}'.",
                GetType(),
                nameof(RunGitCommand),
                gitCommitCommand.GetType().Name,
                gitCommitCommand.Message);
            
            var signature = gitInterface.Repository.Config.BuildSignature(DateTimeOffset.Now);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Created Signature (Name: {name}, Email: {email}, When: {when})",
                GetType(),
                nameof(RunGitCommand),
                signature.Name,
                signature.Email,
                signature.When);
            
            OutputWriter.WriteLine($"Creating commit with message: {gitCommitCommand.Message}");

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Attempting LibGit2Sharp commit...",
                GetType(),
                nameof(RunGitCommand));

            var commit = gitInterface.Repository.Commit(
                gitCommitCommand.Message,
                signature,
                signature,
                GitCommitCommand.DefaultCommitOptions);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: After commit. Commit SHA: {sha}",
                GetType(),
                nameof(RunGitCommand),
                commit.Sha);
            
            return new CommitResult(true, commit, null);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Git Commit failed. Exception message: {exceptionMessage}.",
                GetType(),
                nameof(RunGitCommand),
                e.Message);
            
            return new CommitResult(false, null, e);
        }
    }
}

