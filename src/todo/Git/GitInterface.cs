using System;
using LibGit2Sharp;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    public IGitInterfaceTools GitInterfaceTools { get; }

    public IRepository Repository => _repository.Value;

    private readonly IOutputFolderPathProvider _outputFolderPathProvider;
    private readonly IOutputWriter _outputWriter;
    private readonly Lazy<IRepository> _repository;

    public GitInterface(IOutputFolderPathProvider outputFolderPathProvider,
        IOutputWriter outputWriter, IGitInterfaceTools gitInterfaceTools)
    {
        GitInterfaceTools = gitInterfaceTools;
        _outputFolderPathProvider = outputFolderPathProvider;
        _outputWriter = outputWriter;
        _repository = new Lazy<IRepository>(GetRepository);
    }

    private IRepository GetRepository()
    {
        var repoPath = LibGit2Sharp.Repository.Discover(_outputFolderPathProvider.GetRootedOutputFolder());
        return new Repository(repoPath);
    }

    public TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : GitCommandBase<TResultType>
    {
        return command.ExecuteCommand(this);
    }
}
