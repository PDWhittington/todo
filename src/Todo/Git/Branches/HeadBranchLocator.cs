using LibGit2Sharp;

namespace Todo.Git.Branches;

public class HeadBranchLocator : BranchLocatorBase
{
    public static HeadBranchLocator Instance { get; } = new();

    private HeadBranchLocator() { }

    public override Branch GetBranchForRepository(IRepository repository)
        => repository.Head;

    public override string ToString() => nameof(HeadBranchLocator);
}