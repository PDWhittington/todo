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
        GitGetConflictsCommand gitGetConflictsCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName}.",
            GetType(),
            nameof(RunGitCommand),
            gitGetConflictsCommand.GetType().Name);

        OutputWriter.WriteLine("Retrieving conflicts from git index");

        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Querying LibGit2Sharp Repository.Index.Conflicts...",
                GetType(),
                nameof(RunGitCommand));

            var conflicts = gitInterface.Repository.Index.Conflicts;

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Query of LibGit2Sharp Repository.Index.Conflicts finished.",
                GetType(),
                nameof(RunGitCommand));

            return new ConflictsResult(true, conflicts);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Error while querying of LibGit2Sharp Repository.Index.Conflicts. Exception: {Message}",
                GetType(),
                nameof(RunGitCommand),
                e.Message);

            return new ConflictsResult(false, null, e);
        }
    }
}