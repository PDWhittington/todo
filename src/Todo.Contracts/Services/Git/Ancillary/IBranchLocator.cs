using LibGit2Sharp;

namespace Todo.Contracts.Services.Git.Ancillary;

public interface IBranchLocator
{
    Branch GetBranchForRepository(IRepository repository);
}

