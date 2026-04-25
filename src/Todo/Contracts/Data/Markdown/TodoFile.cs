using System;
using System.Collections.ObjectModel;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Markdown;

public record TodoFile
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ReadOnlyCollection<string> Lines { get; }
    
    private readonly Lazy<MarkdownLineInfo[]> _markdownLines;

    public MarkdownLineInfo[] MarkdownLines => _markdownLines.Value;

    private readonly Lazy<string> _fileContents;
    
    public string FileContents => _fileContents.Value;
    
    private TodoFile(FilePathInfo filePathInfo, string [] lines, Lazy<MarkdownLineInfo[]> markdownLines, Lazy<string> fileContents)
    {
        FilePathInfo = filePathInfo;
        Lines = new ReadOnlyCollection<string>(lines);
        _markdownLines = markdownLines;
        _fileContents = fileContents;
    }

    public static TodoFile Of(FilePathInfo filePathInfo, string [] lines, Lazy<MarkdownLineInfo[]> markdownLines, Lazy<string> fileContents) 
        => new(filePathInfo, lines, markdownLines, fileContents);
}
