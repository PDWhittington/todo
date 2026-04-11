namespace Todo.Contracts.Data.Commands;

public record SyncCommand : CommandBase
{
    public string? CommitMessage { get; }

    private SyncCommand(string? commitMessage)
    {
        if (string.IsNullOrWhiteSpace(commitMessage)) commitMessage = null;

        CommitMessage = commitMessage;
    }

    public static SyncCommand Of(string? commitMessage) => new(commitMessage);
}
