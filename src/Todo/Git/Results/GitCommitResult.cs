using System;
using LibGit2Sharp;

namespace Todo.Git.Results;

public class GitCommitResult(Commit? commit, bool success = true, Exception? exception = null) 
    : GitCommandResult(success, exception)
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Commit? Commit { get; } = commit;
}
