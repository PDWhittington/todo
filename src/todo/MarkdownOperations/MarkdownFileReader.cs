using System;
using System.IO;
using System.Linq;
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
        var lines = File.ReadAllLines(filePathInfo.Path);
        var markdownLines = new Lazy<MarkdownLineInfo[]>(() => lines
            .Select(line => _markdownLineInterpreter.CreateMarkdownLine(line))
            .ToArray());
        
        var fileContents = new Lazy<string>(() => string.Join(Environment.NewLine, lines));
        
        return TodoFile.Of(filePathInfo, lines, markdownLines, fileContents);
    }
}
