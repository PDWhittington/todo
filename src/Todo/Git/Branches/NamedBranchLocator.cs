using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;

namespace Todo.Git.Branches;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class NamedBranchLocator : BranchLocatorBase
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string BranchName { get; }

    public NamedBranchLocator(string branchName)
    {
        BranchName = branchName;
    }

    public override Branch GetBranchForRepository(IRepository repository)
        => repository.Branches[BranchName];
}
