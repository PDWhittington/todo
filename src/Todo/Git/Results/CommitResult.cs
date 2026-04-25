using System;
using LibGit2Sharp;

namespace Todo.Git.Results;

public class CommitResult : VoidResult
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Commit? Commit { get; }

    public CommitResult(bool success, Commit? commit, Exception? exception)
        : base(success, exception)
    {
        Commit = commit;
    }
}
