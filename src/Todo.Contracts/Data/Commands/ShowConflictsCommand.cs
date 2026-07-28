namespace Todo.Contracts.Data.Commands;

public record ShowConflictsCommand : CommandBase
{
    public static ShowConflictsCommand Singleton { get; } = new();

    private ShowConflictsCommand() { }
}