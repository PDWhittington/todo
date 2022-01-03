using System;
using Todo.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IFileNamer
{
    string FileNameWithoutExtension(DateOnly dateOnly);

    string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType);

    string GetFilePath(DateOnly dateOnly, FileTypeEnum fileType);

    string GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType);
}
