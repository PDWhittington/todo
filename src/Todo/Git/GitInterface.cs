using System;
using LibGit2Sharp;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    public Repository Repository => _repository.Value;

    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    private readonly IGitCommandExecutorResolver _gitCommandExecutorResolver;
    private ILogger<GitInterface> Logger { get; }
    private readonly Lazy<Repository> _repository;

    public GitInterface(IOutputFolderPathProvider outputFolderPathProvider, 
        IGitCommandExecutorResolver gitCommandExecutorResolver,
        ILogger<GitInterface> logger)
    {
        _outputFolderPathProvider = outputFolderPathProvider;
        _gitCommandExecutorResolver = gitCommandExecutorResolver;

        Logger = logger;
        _repository = new Lazy<Repository>(GetRepository);
    }

    private Repository GetRepository()
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Querying LibGit2Sharp Repository.Discover.",
            GetType(),
            nameof(RunGitCommand));
        
        var repoPath = Repository.Discover(_outputFolderPathProvider.GetRootedOutputFolder());
        
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Query of LibGit2Sharp Repository.Discover finished. RepositoryPath: {repositoryPath}",
            GetType(),
            nameof(RunGitCommand),
            repoPath);
        
        var repo = new Repository(repoPath); //TODO: currently failing
        
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Repository object created: (WorkingDirectory: {workingDirectory}))",
            GetType(),
            nameof(RunGitCommand),
            repo.Info.WorkingDirectory ?? "<NULL>");

        return repo;
    }

    public TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : IGitCommand<TResultType>
        where TResultType : GitResultBase
    {
        Logger.LogInformation("In {GetType}.{MethodName}: Received command of type {commandType}",
            GetType(), nameof(RunGitCommand), command.GetType());

        var gitCommandExecutor = _gitCommandExecutorResolver.Resolve<TCommandType, TResultType>(command);

        var result = gitCommandExecutor.RunGitCommand(this, command);
        return result;
    }
}
