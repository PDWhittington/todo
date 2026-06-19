using System;
using System.Linq;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Branches;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitFetchCommand : IGitCommand<GitVoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public BranchLocatorBase BranchLocator { get; }

    public GitFetchCommand() : this(new HeadBranchLocator()) { }

    // ReSharper disable once MemberCanBePrivate.Global
    public GitFetchCommand(BranchLocatorBase branchLocator)
    {
        BranchLocator = branchLocator;
    }

    public GitVoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitFetchCommand: Retrieving current branch");

            var currentBranch = BranchLocator.GetBranchForRepository(gitInterface.Repository);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitFetchCommand: Current branch is null:" +
                                                                  (currentBranch is null));

            if (currentBranch is null)
            {
                return new GitVoidResult(false, new InvalidOperationException("No current branch"));
            }

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitFetchCommand: Current branch FriendlyName:" +
                                                                  currentBranch.FriendlyName);

            var remoteName = currentBranch.RemoteName;

            if (string.IsNullOrEmpty(remoteName))
            {
                gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitFetchCommand: Branch has no remote configured");
                return new GitVoidResult(false, new InvalidOperationException("Current branch has no remote tracking information"));
            }

            var remote = gitInterface.Repository.Network.Remotes[remoteName];

            if (remote is null)
            {
                return new GitVoidResult(false, new InvalidOperationException($"Remote '{remoteName}' not found"));
            }

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Fetching from remote {remoteName} ({remote.Url})");

            var refSpecs = remote.FetchRefSpecs.Select(r => r.Specification).ToList();

            if (refSpecs.Count == 0)
            {
                // Fallback to fetching all heads for this remote
                refSpecs.Add($"+refs/heads/*:refs/remotes/{remoteName}/*");
            }

            LibGit2Sharp.Commands.Fetch(
                gitInterface.Repository,
                remoteName,
                refSpecs,
                null,
                $"fetch for sync from {remoteName}");

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("Fetch completed");

            return new GitVoidResult(true, null);
        }
        catch (Exception e)
        {
            return new GitVoidResult(false, e);
        }
    }
}
