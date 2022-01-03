using System;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class MarkdownFileReader : FileReader, IMarkdownFileReader
{
    private readonly IFileNamer _fileNamer;

    public MarkdownFileReader(IFileNamer fileNamer)
    {
        _fileNamer = fileNamer;
    }

    public string ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePath = _fileNamer.GetFilePath(dateOnly, FileTypeEnum.Markdown);

        return GetFileText(filePath);
    }
}
