namespace Todo.Contracts.Data.Commands;

public class PushCommand : CommandBase
{
    public static PushCommand Singleton { get; } = new();

    private PushCommand() { }
}
