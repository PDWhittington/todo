namespace Todo.Contracts.Data.Commands;

public class ShowHelpCommand : CommandBase
{
    public static ShowHelpCommand Singleton { get; } = new();
}
