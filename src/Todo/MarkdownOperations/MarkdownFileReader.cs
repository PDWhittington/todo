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
    private readonly IMarkdownLineInterpreter _markdownLineInterpreter;

    public MarkdownFileReader(IDateListPathResolver dateListPathResolver,
        IMarkdownLineInterpreter markdownLineInterpreter)
    {
        _dateListPathResolver = dateListPathResolver;
        _markdownLineInterpreter = markdownLineInterpreter;
    }

    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = _dateListPathResolver.ResolvePathFor(dateOnly,
            FileTypeEnum.MarkdownDayList, false);
        
        return ReadMarkdownFile(filePathInfo);
    }

    public TodoFile ReadMarkdownFile(FilePathInfo filePathInfo)
    {
        var fileBytes = new Lazy<byte[]>(() =>
            GetFileBytes(filePathInfo.Path));
        
        var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
            _markdownLineInterpreter.CreateMarkdownLine(filePathInfo, fileBytes.Value));
        
        return TodoFile.Of(filePathInfo, markdownLines, fileBytes);
    }
}
