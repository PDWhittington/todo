using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;
using Todo.Git.Branches;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitPushCommand : GitCommandBase<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public BranchLocatorBase BranchLocator { get; }

    public GitPushCommand() : this(new HeadBranchLocator()) { }

    // ReSharper disable once MemberCanBePrivate.Global
    public GitPushCommand(BranchLocatorBase branchLocator)
    {
        BranchLocator = branchLocator;
    }

    internal override VoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            var currentBranch = BranchLocator.GetBranchForRepository(gitInterface.Repository);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Pushing branch {currentBranch.FriendlyName} to origin");

            gitInterface.Repository.Network.Push(currentBranch);

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
