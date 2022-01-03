namespace Todo.Contracts.Data.Commands;

public class CommitCommand : CommandBase
{
    public string CommitMessage { get; }

    private CommitCommand(string commitMessage)
    {
        if (string.IsNullOrWhiteSpace(commitMessage)) commitMessage = null;
        
        CommitMessage = commitMessage;
    }

    public static CommitCommand Of(string commitMessage) => new(commitMessage);
}