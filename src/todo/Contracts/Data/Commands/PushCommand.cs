namespace Todo.Contracts.Data.Commands;

public class PushCommand : CommandBase
{
    public static PushCommand Singleton => new();

    private PushCommand() { }
}