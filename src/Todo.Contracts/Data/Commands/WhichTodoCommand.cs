namespace Todo.Contracts.Data.Commands;

public record WhichTodoCommand : CommandBase
{
    public static WhichTodoCommand Singleton { get; } = new();

    private WhichTodoCommand() { }
}
