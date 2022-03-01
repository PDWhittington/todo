using System;

namespace Todo.Contracts.Data.Commands;

public class UnarchiveCommand : FileMoveCommandBase
{
    private UnarchiveCommand(DateOnly dateOfFileToUnarchive)
        : base(dateOfFileToUnarchive) { }

    public static UnarchiveCommand Of(DateOnly dateOfFileToUnarchive) => new(dateOfFileToUnarchive);
}
