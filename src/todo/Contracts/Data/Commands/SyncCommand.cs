namespace Todo.Contracts.Data.Commands;

public class SyncCommand : CommandBase
{
    public string CommitMessage { get; }

    private SyncCommand(string commitMessage)
    {
        CommitMessage = commitMessage;
    }

    public static SyncCommand Of(string commitMessage) => new(commitMessage);
}