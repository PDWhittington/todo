using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public record GitGetRepoInfoCommand : IGitCommand<RepoInfoResult>
{
    // ReSharper disable once UnusedMember.Global
    public static readonly GitGetRepoInfoCommand Instance = new();
    
    private GitGetRepoInfoCommand() { }
}