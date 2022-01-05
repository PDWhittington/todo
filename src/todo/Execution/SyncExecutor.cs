using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class SyncExecutor : CommandExecutorBase<SyncCommand>, ISyncExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ICommitExecutor _commitExecutor;
    private readonly IPushExecutor _pushExecutor;

    public SyncExecutor(IConfigurationProvider configurationProvider, ICommitExecutor commitExecutor, IPushExecutor pushExecutor)
    {
        _configurationProvider = configurationProvider;
        _commitExecutor = commitExecutor;
        _pushExecutor = pushExecutor;
    }

    public override void Execute(SyncCommand syncCommand)
    {
        if (!_configurationProvider.Config.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        _commitExecutor.Execute(CommitCommand.Of(syncCommand.CommitMessage));
        _pushExecutor.Execute(PushCommand.Singleton);
    }
}
