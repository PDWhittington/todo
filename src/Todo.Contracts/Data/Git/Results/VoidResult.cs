namespace Todo.Contracts.Data.Git.Results;

public record VoidResult(bool Success, Exception? Exception = null)
    : GitResultBase(Success, Exception);

