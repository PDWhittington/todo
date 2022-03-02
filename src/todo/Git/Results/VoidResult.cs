using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Git.Results;

public class VoidResult
{
    public bool Success { get; }
    public Exception? Exception { get; }

    public VoidResult(bool success, Exception? exception)
    {
        if (!Validate(success, exception))
            throw new ArgumentException("If success is true, there must be no exception. " +
                                        "Or if success is false, there must be an exception");
        Success = success;
        Exception = exception;
    }

    private static bool Validate(bool success, Exception? exception)
        => success ^ (exception != null);
}
