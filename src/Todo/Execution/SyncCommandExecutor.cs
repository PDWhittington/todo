using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandExecutor(
    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    IConfigurationProvider configurationProvider,
    ICommitCommandExecutor commitExecutor,
    IPushCommandExecutor pushExecutor,
    IOutputWriter outputWriter,
    ILogger<SyncCommandExecutor> logger)
    : CommandExecutorBase<SyncCommand>(outputWriter, logger), ISyncCommandExecutor
{
    public override void Execute(SyncCommand syncCommand)
    {
        if (!configurationProvider.ConfigInfo.Configuration.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        OutputWriter.WriteLine("Executing a commit and a push command.");

        commitExecutor.Execute(CommitCommand.Of(syncCommand.CommitMessage));
        pushExecutor.Execute(PushCommand.Singleton);
    }
}