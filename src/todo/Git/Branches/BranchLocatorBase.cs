using LibGit2Sharp;

namespace Todo.Git.Branches;

public abstract class BranchLocatorBase
{
    public abstract Branch GetBranchForRepository(IRepository repository);
}
