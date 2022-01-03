using System;

namespace Todo.Contracts.Data.Commands;

public class ArchiveCommand : CommandBase
{
    public DateOnly DateOfFileToArchive { get; }

    private ArchiveCommand(DateOnly dateOfFileToArchive)
    {
        DateOfFileToArchive = dateOfFileToArchive;
    }

    public static ArchiveCommand Of(DateOnly dateOfFileToArchive) => new(dateOfFileToArchive);
}