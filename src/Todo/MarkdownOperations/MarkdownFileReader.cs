using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.FileSystem;

namespace Todo.MarkdownOperations;

public class MarkdownFileReader(
    IDateListPathResolver dateListPathResolver,
    IMarkdownLineInterpreter markdownLineInterpreter)
    : FileReaderBase, IMarkdownFileReader
{
    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = dateListPathResolver.ResolvePathFor(dateOnly,
            FileTypeEnum.MarkdownDayList, false);
        
        return ReadMarkdownFile(filePathInfo);
    }

    public TodoFile ReadMarkdownFile(FilePathInfo filePathInfo)
    {
        var fileBytes = new Lazy<byte[]>(() =>
            GetFileBytes(filePathInfo.Path));
        
        var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
            markdownLineInterpreter.CreateMarkdownLine(filePathInfo, fileBytes.Value));
        
        return TodoFile.Of(filePathInfo, markdownLines, fileBytes);
    }
}
