namespace Todo.Contracts.Data.Commands;

public abstract record FileMoveCommandBase : CommandBase
{
    public DateOnly DateOfFileToArchive { get; }

    protected FileMoveCommandBase(DateOnly dateOfFileToArchive)
    {
        DateOfFileToArchive = dateOfFileToArchive;
    }
}
