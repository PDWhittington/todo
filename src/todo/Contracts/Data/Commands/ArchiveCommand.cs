using System;

namespace Todo.Contracts.Data.Commands;

public record ArchiveCommand : FileMoveCommandBase
{
    private ArchiveCommand(DateOnly dateOfFileToArchive)
        : base(dateOfFileToArchive) { }

    public static ArchiveCommand Of(DateOnly dateOfFileToArchive) => new(dateOfFileToArchive);
}
