using System;
using LibGit2Sharp;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Git.Commands;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    public IGitInterfaceTools GitInterfaceTools { get; }

    public Repository Repository => _repository.Value;

    private readonly IOutputFolderPathProvider _outputFolderPathProvider;
    private readonly Lazy<Repository> _repository;

    public GitInterface(IOutputFolderPathProvider outputFolderPathProvider, IGitInterfaceTools gitInterfaceTools)
    {
        GitInterfaceTools = gitInterfaceTools;
        _outputFolderPathProvider = outputFolderPathProvider;
        _repository = new Lazy<Repository>(GetRepository);
    }

    private Repository GetRepository()
    {
        var repoPath = Repository.Discover(_outputFolderPathProvider.GetRootedOutputFolder());
        return new Repository(repoPath);
    }

    public TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : IGitCommand<TResultType>
    {
        return command.ExecuteCommand(this);
    }
}
