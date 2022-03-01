using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public class GitMoveCommand : GitCommandBase
{
    public string SourcePath { get; }
    public string DestinationPath { get; }

    public GitMoveCommand(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    internal override bool ExecuteCommand(IGitInterface gitInterface)
    {
        new GitAddCommand(SourcePath).ExecuteCommand(gitInterface);
        return gitInterface.RunSpecialGitCommand($"mv {SourcePath} {DestinationPath}");
    }
}
