using System;

namespace Todo.Contracts.Services.FileNaming;

public interface IFileNamer
{
    string FileNameForDate(DateOnly dateOnly);

    string FilePathForDate(DateOnly dateOnly);

    string ArchiveFilePathForDate(DateOnly dateOnly);
}