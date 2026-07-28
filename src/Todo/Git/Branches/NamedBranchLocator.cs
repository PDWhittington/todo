using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;

namespace Todo.Git.Branches;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class NamedBranchLocator(string branchName) : BranchLocatorBase
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string BranchName { get; } = branchName;

    public override Branch GetBranchForRepository(IRepository repository)
        => repository.Branches[BranchName];

    public override string ToString()
        => $"{nameof(NamedBranchLocator)}: Branch={BranchName}";
}
