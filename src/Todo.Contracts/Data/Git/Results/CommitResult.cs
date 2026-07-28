using LibGit2Sharp;

namespace Todo.Contracts.Data.Git.Results;

public record CommitResult(bool Success, Commit? Commit, Exception? Exception = null) 
    : GitResultBase(Success, Exception)
{
    // ReSharper disable once UnusedMember.Global
    public Commit? Commit { get; } = Commit;
}