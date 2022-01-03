using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class CommitExecutor : ICommitExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;

    public CommitExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
    }
    
    public void Execute(CommitCommand commitCommand)
    {
        var configuration = _configurationProvider.GetConfiguration();

        if (!configuration.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        var commitMessage = commitCommand.CommitMessage ?? $"Synced as at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            
        _gitInterface.RunGitCommand("reset");
        _gitInterface.RunGitCommand($"add \"{configuration.OutputFolder}\"");
        _gitInterface.RunGitCommand($"commit -m \"{commitMessage}\"");
    }
}