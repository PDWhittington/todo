using System;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;
using Todo.UI;

namespace Todo.Git.Execution;

public class GitResetCommandExecutor(IOutputWriter outputWriter)
    : GitCommandExecutorBase<GitResetCommand, VoidResult>(outputWriter),
        IGitResetCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, GitResetCommand command)
    {
        try
        {
            gitInterface.Repository.Reset(command.Hard ? ResetMode.Hard : ResetMode.Soft);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

