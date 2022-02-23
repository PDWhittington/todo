using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandExecutor : CommandExecutorBase<CommitCommand>, ICommitCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    public CommitCommandExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface,
        IOutputFolderPathProvider outputFolderPathProvider)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
        _outputFolderPathProvider = outputFolderPathProvider;
    }

    public override void Execute(CommitCommand commitCommand)
    {
        if (!_configurationProvider.Config.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        var commitMessage = commitCommand.CommitMessage ?? $"Synced as at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        _gitInterface.RunGitCommand("reset");
        _gitInterface.RunGitCommand($"add \"{_outputFolderPathProvider.GetRootedOutputFolder()}\"");
        //Archive may not be nested within the OutputFolder
        _gitInterface.RunGitCommand($"add \"{_outputFolderPathProvider.GetRootedArchiveFolder()}\"");
        _gitInterface.RunGitCommand($"commit -m \"{commitMessage}\"");
    }
}
