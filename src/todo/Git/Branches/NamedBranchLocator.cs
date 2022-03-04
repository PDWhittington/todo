using LibGit2Sharp;

namespace Todo.Git.Branches;

public class NamedBranchLocator : BranchLocatorBase
{
    public string BranchName { get; }

    public NamedBranchLocator(string branchName)
    {
        BranchName = branchName;
    }

    public override Branch GetBranchForRepository(IRepository repository)
        => repository.Branches[BranchName];
}
