using System;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitCommitCommand(string Message) : GitCommandBase<CommitResult>
{
    internal override CommitResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            var signature = gitInterface.Repository.Config.BuildSignature(DateTimeOffset.Now);

            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Creating commit with message: {Message}");

            var commit = gitInterface.Repository.Commit(Message, signature, signature);

            return new CommitResult(true, commit, null);
        }
        catch(Exception e)
        {
            return new CommitResult(false, null, e);
        }
    }
}
