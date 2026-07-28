using LibGit2Sharp;

namespace Todo.Contracts.Data.Git.Results;

public record RepoInfoResult(
    bool Success,
    // ReSharper disable once NotAccessedPositionalProperty.Global
    RepositoryInformation? RepositoryInformation,
    Exception? Exception = null ) 
    : GitResultBase(Success, Exception);