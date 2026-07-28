using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitGetRepoInfoCommandExecutor(IOutputWriter outputWriter, ILogger<GitGetRepoInfoCommandExecutor> logger)
    : GitCommandExecutorBase<GitGetRepoInfoCommand, RepoInfoResult>(outputWriter, logger),
        IGitGetRepoInfoCommandExecutor
{
    public override RepoInfoResult RunGitCommand(
        IGitInterface gitInterface,
        GitGetRepoInfoCommand gitGetRepoInfoCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName}.",
            GetType(),
            nameof(RunGitCommand),
            gitGetRepoInfoCommand.GetType().Name);
        
        OutputWriter.WriteLine("Retrieving repo information...");

        try
        {
            Logger.LogInformation(
                "In {GetType}.{MethodName}: Querying LibGit2Sharp Repository.Index.Conflicts...",
                GetType(),
                nameof(RunGitCommand));
            
            var repositoryInformation = gitInterface.Repository.Info;
            return new RepoInfoResult(true, repositoryInformation);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Error while querying LibGit2Sharp Repository.Index.Conflicts. Exception: {Message}",
                GetType(),
                nameof(RunGitCommand),
                e.Message);
            
            return new RepoInfoResult(true, null, e);
        }
    }
}