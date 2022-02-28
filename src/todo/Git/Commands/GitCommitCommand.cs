namespace Todo.Git.Commands;

public class GitCommitCommand : GitCommandBase
{
    public string Message { get; }

    public GitCommitCommand(string message)
    {
        Message = message;
    }

    internal override string GetCommand() => $"commit -m {Message}";
}
