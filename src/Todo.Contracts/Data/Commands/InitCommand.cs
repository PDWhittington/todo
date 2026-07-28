namespace Todo.Contracts.Data.Commands;

public record InitCommand : CommandBase
{
    public static InitCommand Singleton { get; } = new();

    private InitCommand() { }
}