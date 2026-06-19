using System;
using LibGit2Sharp;

namespace Todo.Git.Results;

public class GitRebaseResult(
    bool success = true,
    string message = "",
    RebaseStatus? rebaseStatus = null,
    Exception? exception = null)
    : GitCommandResult(success, exception)
{
    public string Message { get; } = message;
    public RebaseStatus? RebaseStatus { get; } = rebaseStatus;
}