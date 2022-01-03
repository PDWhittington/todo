using System;

namespace Todo.Contracts.Services.FileNaming;

public interface IFileNamer
{
    string MarkdownFileNameForDate(DateOnly dateOnly);

    string MarkdownFilePathForDate(DateOnly dateOnly);

    string MarkdownArchiveFilePathForDate(DateOnly dateOnly);
}