using LibGit2Sharp;

namespace Todo.Contracts.Data.Git.Results;

public record RepoInfoResult(
    bool Success,
    RepositoryInformation? RepositoryInformation,
    Exception? Exception = null
) : GitResultBase(Success, Exception);