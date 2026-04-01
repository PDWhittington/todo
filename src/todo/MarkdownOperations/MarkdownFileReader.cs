using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.FileSystem;

namespace Todo.MarkdownOperations;

public class MarkdownFileReader : FileReaderBase, IMarkdownFileReader
{
    private readonly IDateListPathResolver _dateListPathResolver;

    public MarkdownFileReader(IDateListPathResolver dateListPathResolver)
    {
        _dateListPathResolver = dateListPathResolver;
    }

    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = _dateListPathResolver.ResolvePathFor(dateOnly,
            FileTypeEnum.MarkdownDayList, false);
        
        return ReadMarkdownFile(filePathInfo);
    }

    public TodoFile ReadMarkdownFile(FilePathInfo filePathInfo)
    {
        
    }

}
