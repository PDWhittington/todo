using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class MarkdownFileReader : FileReader, IMarkdownFileReader
{
    private readonly IDateListPathResolver _dateListPathResolver;

    public MarkdownFileReader(IDateListPathResolver dateListPathResolver)
    {
        _dateListPathResolver = dateListPathResolver;
    }

    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = _dateListPathResolver.ResolvePathFor(dateOnly, FileTypeEnum.Markdown, false);

        return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path));
    }
}
