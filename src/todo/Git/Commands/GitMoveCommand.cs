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

    internal override string GetCommand() => $"mv {SourcePath} {DestinationPath}";
}
