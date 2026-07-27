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
        GitGetRepoInfoCommand command
    )
    {
        OutputWriter.WriteLine("Retrieving repo information...");

        try
        {
            var repositoryInformation = gitInterface.Repository.Info;
            return new RepoInfoResult(true, repositoryInformation);
        }
        catch (Exception e)
        {
            return new RepoInfoResult(true, null, e);
        }
    }
}

