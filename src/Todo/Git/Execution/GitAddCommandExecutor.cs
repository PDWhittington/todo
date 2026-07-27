using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;
using Todo.UI;

namespace Todo.Git.Execution;

public class GitAddCommandExecutor(IOutputWriter outputWriter)
    : GitCommandExecutorBase<GitAddCommand, VoidResult>(outputWriter),
        IGitAddCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, GitAddCommand command)
    {
        try
        {
            OutputWriter.WriteLine($"Staging {command.Path}");

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, command.Path);
            return new VoidResult(true);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

