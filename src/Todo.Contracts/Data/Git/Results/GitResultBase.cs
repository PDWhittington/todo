namespace Todo.Contracts.Data.Git.Results;

public abstract record GitResultBase
{
    public bool Success { get; }
    public Exception? Exception { get; }

    protected GitResultBase(bool success, Exception? exception = null)
    {
        if (!Validate(success, exception))
            throw new ArgumentException(
                "If success is true, there must be no exception. "
                    + "Or if success is false, there must be an exception"
            );
        Success = success;
        Exception = exception;
    }

    private static bool Validate(bool success, Exception? exception) =>
        success ^ exception is not null;
}

