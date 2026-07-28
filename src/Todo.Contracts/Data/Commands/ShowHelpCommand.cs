namespace Todo.Contracts.Data.Commands;

public record ShowHelpCommand : CommandBase
{
    public static ShowHelpCommand Singleton { get; } = new();
}