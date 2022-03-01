using System;

namespace Todo.Contracts.Data.Commands;

public class ArchiveCommand : FileMoveCommandBase
{
    private ArchiveCommand(DateOnly dateOfFileToArchive)
        : base(dateOfFileToArchive) { }

    public static ArchiveCommand Of(DateOnly dateOfFileToArchive) => new(dateOfFileToArchive);
}
