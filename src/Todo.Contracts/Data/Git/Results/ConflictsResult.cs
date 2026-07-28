using LibGit2Sharp;

namespace Todo.Contracts.Data.Git.Results;

public record ConflictsResult(
    bool Success,
    ConflictCollection? ConflictCollection,
    Exception? Exception = null
) : GitResultBase(Success, Exception);