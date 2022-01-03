using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class MarkdownFileReader : FileReader, IMarkdownFileReader
{
    private readonly IContentFileResolver _contentFileResolver;


    public MarkdownFileReader(IContentFileResolver contentFileResolver)
    {
        _contentFileResolver = contentFileResolver;
    }

    public string ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePath = _contentFileResolver.GetPathFor(dateOnly, FileTypeEnum.Markdown, false);

        return GetFileText(filePath.Path);
    }
}
