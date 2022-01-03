using System;
using Todo.Contracts.Data.FileSystem;
using Todo.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IFileNamer
{
    string FileNameWithoutExtension(DateOnly dateOnly);

    string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType);

    FilePathInfo GetFilePath(DateOnly dateOnly, FileTypeEnum fileType);

    FilePathInfo GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType);
}
