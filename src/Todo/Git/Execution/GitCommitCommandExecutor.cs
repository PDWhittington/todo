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
    public override CommitResult RunGitCommand(IGitInterface gitInterface, GitCommitCommand gitCommitCommand)
    {
        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Received commit with override message '{commitMessage}'.",
                GetType(),
                nameof(RunGitCommand),
                gitCommitCommand.Message
            );
            
            var signature = gitInterface.Repository.Config.BuildSignature(DateTimeOffset.Now);

            OutputWriter.WriteLine($"Creating commit with message: {gitCommitCommand.Message}");

            var commit = gitInterface.Repository.Commit(
                gitCommitCommand.Message,
                signature,
                signature,
                GitCommitCommand.DefaultCommitOptions
            );

            return new CommitResult(true, commit, null);
        }
        catch (Exception e)
        {
            return new CommitResult(false, null, e);
        }
    }
}

