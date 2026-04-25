using LibGit2Sharp;

namespace Todo.Git.Branches;

public class HeadBranchLocator : BranchLocatorBase
{
    public override Branch GetBranchForRepository(IRepository repository)
        => repository.Head;
}
