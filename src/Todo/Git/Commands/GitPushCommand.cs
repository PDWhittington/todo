using System;
using Todo.Contracts.Services.Git;
using Todo.Git.Branches;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitPushCommand : IGitCommand<GitVoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public BranchLocatorBase BranchLocator { get; }

    public GitPushCommand() : this(new HeadBranchLocator()) { }

    // ReSharper disable once MemberCanBePrivate.Global
    public GitPushCommand(BranchLocatorBase branchLocator)
    {
        BranchLocator = branchLocator;
    }

    public GitVoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitPushCommand: Retrieving current branch");
            
            var currentBranch = BranchLocator.GetBranchForRepository(gitInterface.Repository);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitPushCommand: Current branch is null:" + 
                                                                  (currentBranch is null));
            
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine("GitPushCommand: Current branch FriendlyName:" + 
                                                                  currentBranch!.FriendlyName);  
            
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Pushing branch {currentBranch.FriendlyName} to origin");

            gitInterface.Repository.Network.Push(currentBranch);

            return new GitVoidResult(true, null);
        }
        catch (Exception e)
        {
            return new GitVoidResult(false, e);
        }
    }
}
