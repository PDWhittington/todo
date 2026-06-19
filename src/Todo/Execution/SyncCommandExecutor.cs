using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;
using Todo.Git.Results;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandExecutor(
    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    IConfigurationProvider configurationProvider,
    ICommitCommandExecutor commitExecutor,
    IPushCommandExecutor pushExecutor,
    IGitInterface gitInterface,
    IOutputWriter outputWriter)
    : CommandExecutorBase<SyncCommand>(outputWriter), ISyncCommandExecutor
{
    public override void Execute(SyncCommand syncCommand)
    {
        if (!configurationProvider.ConfigInfo.Configuration.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        OutputWriter.WriteLine("Executing a commit and a push command.");

        commitExecutor.Execute(CommitCommand.Of(syncCommand.CommitMessage));

        var currentBranch = gitInterface.Repository.Head;

        if (currentBranch is not { IsTracking: true })
        {
            OutputWriter.WriteLine("Current branch has no upstream tracking information configured. Push skipped.");
            return;
        }

        var fetchResult = gitInterface.RunGitCommand<GitFetchCommand, GitVoidResult>(new GitFetchCommand());

        if (!fetchResult.Success)
        {
            OutputWriter.WriteLine("Fetch did not complete successfully.");
            return;
        }

        var trackingDetails = currentBranch.TrackingDetails;
        var behindBy = trackingDetails.BehindBy ?? 0;

        if (behindBy > 0)
        {
            OutputWriter.WriteLine($"Remote has {behindBy} commit(s) not present locally.");

            var localTip = currentBranch.Tip;
            var remoteTip = currentBranch.TrackedBranch?.Tip;

            if (localTip != null && remoteTip != null)
            {

                OutputWriter.WriteLine("Rebasing local commits onto remote...");

                var rebaseResult =
                    gitInterface.RunGitCommand<GitRebaseCommand, GitRebaseResult>(new GitRebaseCommand());

                if (!rebaseResult.Success)
                {
                    OutputWriter.WriteLine("Rebase encountered an unexpected error.");
                    return;
                }

                OutputWriter.WriteLine(
                    "Rebase cannot be performed without conflicts. " +
                    "Please resolve the conflict manually (for example by running 'git pull --rebase' or 'git pull' on the command line) and then retry.");
                return;
            }

            OutputWriter.WriteLine("Unable to determine remote tip; skipping rebase.");
            return;
        }

        pushExecutor.Execute(PushCommand.Singleton);
    }
}
