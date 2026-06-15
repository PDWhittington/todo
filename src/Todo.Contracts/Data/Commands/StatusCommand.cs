namespace Todo.Contracts.Data.Commands;

public record StatusCommand : CommandBase
{
    public static StatusCommand Singleton { get; } = new();

    private StatusCommand() { }
}

