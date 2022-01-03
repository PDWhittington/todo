using System;
using Todo.FileNaming;

namespace Todo.Contracts.Services.FileNaming;

public interface IFileNamer
{
    public string FileNameWithoutExtension(DateOnly dateOnly);
    
    public string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType);

    public string GetFilePath(DateOnly dateOnly, FileTypeEnum fileType);

    public string GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType);
}