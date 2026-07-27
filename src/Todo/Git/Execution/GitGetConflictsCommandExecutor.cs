using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitGetConflictsCommandExecutor(IOutputWriter outputWriter, ILogger<GitGetConflictsCommandExecutor> logger)
    : GitCommandExecutorBase<GitGetConflictsCommand, ConflictsResult>(outputWriter, logger),
        IGitGetConflictsCommandExecutor
{
    public override ConflictsResult RunGitCommand(
        IGitInterface gitInterface,
        GitGetConflictsCommand command
    )
    {
        OutputWriter.WriteLine("Retrieving conflicts from git index");

        try
        {
            var conflicts = gitInterface.Repository.Index.Conflicts;
            return new ConflictsResult(true, conflicts);
        }
        catch (Exception e)
        {
            return new ConflictsResult(false, null, e);
        }
    }
}

