namespace Todo.Contracts.Data.Commands;

public class InitCommand : CommandBase
{
    public static InitCommand Singleton { get; } = new();

    private InitCommand() { }
}
