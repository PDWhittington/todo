namespace Todo.Contracts.Data.Commands;

public record OpenTodoFolderCommand : CommandBase
{
    public static OpenTodoFolderCommand Singleton { get; } = new();
}