using LibGit2Sharp;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitCommitCommand(string Message, CommitOptions CommitOptions) 
    : IGitCommand<CommitResult>
{
    public GitCommitCommand(string message)
        : this(message, DefaultCommitOptions)
    { }

    public static CommitOptions DefaultCommitOptions { get; } = new()
    {
        AllowEmptyCommit = false,
        AmendPreviousCommit = false,
        CommentaryChar = '#',
        PrettifyMessage = true
    };
}