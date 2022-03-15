namespace Todo.Contracts.Data.Commands;

public class WhichTodoCommand : CommandBase
{
    public static WhichTodoCommand Singleton { get; } = new();

    private WhichTodoCommand() { }
}
