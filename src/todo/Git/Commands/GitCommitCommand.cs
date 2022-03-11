using System;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitCommitCommand : GitCommandBase<CommitResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string Message { get; }

    public GitCommitCommand(string message)
    {
        Message = message;
    }

    internal override CommitResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            var signature = repo.Config.BuildSignature(DateTimeOffset.Now);

            outputWriter?.WriteLine($"Creating commit with message: {Message}");

            var commit = repo.Commit(Message, signature, signature);

            return new CommitResult(true, commit, null);
        }
        catch(Exception e)
        {
            return new CommitResult(false, null, e);
        }
    }
}
