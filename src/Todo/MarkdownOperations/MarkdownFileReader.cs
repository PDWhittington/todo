using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.FileSystem;

namespace Todo.MarkdownOperations;

public class MarkdownFileReader(
    IDateListPathResolver dateListPathResolver,
    IMarkdownLineInterpreter markdownLineInterpreter,
    IUnmanagedByteArrayManager unmanagedByteArrayManager)
    : FileReaderBase(unmanagedByteArrayManager), IMarkdownFileReader
{
    public TodoFile ReadMarkdownFile(DateOnly dateOnly)
    {
        var filePathInfo = dateListPathResolver.ResolvePathFor(dateOnly,
            FileTypeEnum.MarkdownDayList, false);

        return ReadMarkdownFile(filePathInfo);
    }

    public TodoFile ReadMarkdownFile(FilePathInfo filePathInfo)
    {
        var lazyFile = new Lazy<UnmanagedByteArray>(() =>
            LoadFile(filePathInfo.Path));

        var markdownLines = new Lazy<MarkdownLineInfo[]>(() => 
            markdownLineInterpreter.CreateMarkdownLines(lazyFile.Value));

        return TodoFile.Of(filePathInfo, markdownLines, lazyFile);
    }
}