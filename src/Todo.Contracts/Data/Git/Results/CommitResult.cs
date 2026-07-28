using LibGit2Sharp;

namespace Todo.Contracts.Data.Git.Results;

public record CommitResult(bool Success, Commit? Commit, Exception? Exception = null) 
    : GitResultBase(Success, Exception)
{
    public Commit? Commit { get; } = Commit;
}