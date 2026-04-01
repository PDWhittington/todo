using System;
using System.Collections;
using System.Linq;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Markdown;

public class TodoFile
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }

    public MarkdownLineInfo [] MarkdownLines { get; }
    
    public string FileContents { get; }
    
    // ReSharper disable once MemberCanBePrivate.Global
    private TodoFile(FilePathInfo filePathInfo, MarkdownLineInfo [] markdownLines)
    {
        FilePathInfo = filePathInfo;
        MarkdownLines = markdownLines;
        FileContents = string.Join(Environment.NewLine, markdownLines.Select(x => x.Line));
    }

    public static TodoFile Of(FilePathInfo filePathInfo, MarkdownLineInfo [] fileLines) 
        => new(filePathInfo, fileLines);
}
