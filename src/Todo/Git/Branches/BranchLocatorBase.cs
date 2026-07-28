using LibGit2Sharp;
using Todo.Contracts.Services.Git.Ancillary;

namespace Todo.Git.Branches;

public abstract class BranchLocatorBase : IBranchLocator
{
    public abstract Branch GetBranchForRepository(IRepository repository);
}