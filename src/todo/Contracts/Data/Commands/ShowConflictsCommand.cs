namespace Todo.Contracts.Data.Commands;

public class ShowConflictsCommand : CommandBase
{
    public static ShowConflictsCommand Singleton { get; } = new();

    private ShowConflictsCommand() { }
}
