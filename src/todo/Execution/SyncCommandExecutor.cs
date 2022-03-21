using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandExecutor : CommandExecutorBase<SyncCommand>, ISyncCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ICommitCommandExecutor _commitExecutor;
    private readonly IPushCommandExecutor _pushExecutor;

    public SyncCommandExecutor(IConfigurationProvider configurationProvider,
        ICommitCommandExecutor commitExecutor, IPushCommandExecutor pushExecutor,
        IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _commitExecutor = commitExecutor;
        _pushExecutor = pushExecutor;
    }

    public override void Execute(SyncCommand syncCommand)
    {
        if (!_configurationProvider.ConfigInfo.Configuration.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        OutputWriter.WriteLine("Executing a commit and a push command.");

        _commitExecutor.Execute(CommitCommand.Of(syncCommand.CommitMessage));
        _pushExecutor.Execute(PushCommand.Singleton);
    }
}
