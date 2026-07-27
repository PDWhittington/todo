using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitAddCommandExecutor(IOutputWriter outputWriter, ILogger<GitAddCommandExecutor> logger)
    : GitCommandExecutorBase<GitAddCommand, VoidResult>(outputWriter, logger),
        IGitAddCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, GitAddCommand gitAddCommand)
    {
        try
        {
            OutputWriter.WriteLine($"Staging {gitAddCommand.Path}");

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitAddCommand.Path);
            return new VoidResult(true);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

