namespace Todo.Contracts.Data.Commands;

public record PushCommand : CommandBase
{
    public static PushCommand Singleton { get; } = new();

    private PushCommand() { }
}
