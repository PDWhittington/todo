using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Branches;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitRebaseCommand(BranchLocatorBase BranchLocator) : IGitCommand<GitRebaseResult>
{
    public GitRebaseCommand() : this(new HeadBranchLocator()) { }

    public GitRebaseResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitRebaseCommand: Retrieving current branch");

            var currentBranch = BranchLocator.GetBranchForRepository(gitInterface.Repository);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"GitRebaseCommand: Current branch {currentBranch.FriendlyName}");

            if (!currentBranch.IsTracking)
            {
                return new GitRebaseResult(false, "Current branch is not tracking a remote branch");
            }

            var upstream = currentBranch.TrackedBranch;

            if (upstream is null)
            {
                return new GitRebaseResult(false, "No tracked branch found");
            }

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"GitRebaseCommand: Rebasing onto {upstream.FriendlyName}");

            var signature = gitInterface.Repository.Config.BuildSignature(DateTimeOffset.Now);
            var identity = new Identity(signature.Name, signature.Email);

            var rebaseResult = gitInterface.Repository.Rebase.Start(
                currentBranch,
                upstream,
                upstream,
                identity,
                null);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"GitRebaseCommand: Rebase status: {rebaseResult.Status}, " +
                $"completed {rebaseResult.CompletedStepCount}/{rebaseResult.TotalStepCount}");

            switch (rebaseResult.Status)
            {
                case RebaseStatus.Conflicts:
                    gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                        "GitRebaseCommand: Rebase encountered conflicts, aborting");

                    gitInterface.Repository.Rebase.Abort();

                    return new GitRebaseResult(false, "Rebase failed due to conflicts", 
                        RebaseStatus.Conflicts);
                
                case RebaseStatus.Stop:
                    gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                        "GitRebaseCommand: Rebase stopped, aborting");

                    gitInterface.Repository.Rebase.Abort();

                    return new GitRebaseResult(false, "Rebase stopped unexpectedly",
                        RebaseStatus.Stop);

                case RebaseStatus.Complete:
                    gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitRebaseCommand: Rebase completed successfully");

                    return new GitRebaseResult(true, "", RebaseStatus.Complete);
                
                default:
                    return new GitRebaseResult(false, "Unknown rebase result");  
            }
        }
        catch (Exception e)
        {
            // Attempt to abort if a rebase might be in progress
            try
            {
                gitInterface.Repository.Rebase.Abort();
            }
            catch(Exception abortException)
            {
                return new GitRebaseResult(false, "Unknown rebasing error and cannot abort", 
                    null, abortException);
            }

            return new GitRebaseResult(false, "Unknown rebasing error", null, e);
        }
    }
}
