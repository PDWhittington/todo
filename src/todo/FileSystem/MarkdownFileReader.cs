using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class MarkdownFileReader : FileReader, IMarkdownFileReader
{
    private readonly IContentFilePathResolver _contentFileResolver;


    public MarkdownFileReader(IContentFilePathResolver contentFileResolver)
    {
        _contentFileResolver = contentFileResolver;
    }

    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = _contentFileResolver.GetPathFor(dateOnly, FileTypeEnum.Markdown, false);

        return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path));
    }
}
