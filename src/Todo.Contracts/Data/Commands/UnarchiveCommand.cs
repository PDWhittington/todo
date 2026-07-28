namespace Todo.Contracts.Data.Commands;

public record UnarchiveCommand : FileMoveCommandBase
{
    private UnarchiveCommand(DateOnly dateOfFileToUnarchive)
        : base(dateOfFileToUnarchive) { }

    public static UnarchiveCommand Of(DateOnly dateOfFileToUnarchive) => new(dateOfFileToUnarchive);
}