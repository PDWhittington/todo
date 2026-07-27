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
    public IGitInterfaceTools GitInterfaceTools { get; }

    public Repository Repository => _repository.Value;

    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    private readonly IGitCommandExecutorResolver _gitCommandExecutorResolver;
    private ILogger<GitInterface> Logger { get; }
    private readonly Lazy<Repository> _repository;

    public GitInterface(IOutputFolderPathProvider outputFolderPathProvider, 
        IGitInterfaceTools gitInterfaceTools,
        IGitCommandExecutorResolver gitCommandExecutorResolver,
        ILogger<GitInterface> logger)
    {
        GitInterfaceTools = gitInterfaceTools;
        _outputFolderPathProvider = outputFolderPathProvider;
        _gitCommandExecutorResolver = gitCommandExecutorResolver;

        Logger = logger;
        _repository = new Lazy<Repository>(GetRepository);
    }

    private Repository GetRepository()
    {
        var repoPath = Repository.Discover(_outputFolderPathProvider.GetRootedOutputFolder());
        return new Repository(repoPath);
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
