using LibGit2Sharp;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitCommitCommand(string Message, CommitOptions CommitOptions) 
    : IGitCommand<CommitResult>
{
    public CommitOptions CommitOptions { get; } = CommitOptions;
    
    public GitCommitCommand(string message)
        : this(message, DefaultCommitOptions)
    { }

    private static CommitOptions DefaultCommitOptions { get; } = new()
    {
        AllowEmptyCommit = false,
        AmendPreviousCommit = false,
        CommentaryChar = '#',
        PrettifyMessage = true
    };
}