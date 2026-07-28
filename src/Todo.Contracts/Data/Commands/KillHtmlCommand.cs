namespace Todo.Contracts.Data.Commands;

public record KillHtmlCommand : CommandBase
{
    public static KillHtmlCommand Singleton { get; } = new();

    private KillHtmlCommand() { }
}