using System;

namespace Todo.Contracts.Data.Commands;

public abstract class FileMoveCommandBase : CommandBase
{
    public DateOnly DateOfFileToArchive { get; }

    protected FileMoveCommandBase(DateOnly dateOfFileToArchive)
    {
        DateOfFileToArchive = dateOfFileToArchive;
    }
}
