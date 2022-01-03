using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class SyncExecutor : ISyncExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;

    public SyncExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
    }

    public void Execute(SyncCommand syncCommand)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var commitMessage = syncCommand.CommitMessage ?? $"Synced as at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            
        _gitInterface.RunGitCommand("reset");
        _gitInterface.RunGitCommand($"add \"{configuration.OutputFolder}\"");
        _gitInterface.RunGitCommand($"commit -m \"{commitMessage}\"");
        _gitInterface.RunGitCommand($"push");
    }
}