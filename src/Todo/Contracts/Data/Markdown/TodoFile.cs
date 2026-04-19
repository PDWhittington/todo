using System;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Markdown;

public record TodoFile
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }
    
    private readonly Lazy<MarkdownLineInfo[]> _markdownLines;

    public MarkdownLineInfo[] MarkdownLines => _markdownLines.Value;

    private readonly Lazy<byte[]> _fileContents;
    
    public byte [] FileContents => _fileContents.Value;
    
    private TodoFile(FilePathInfo filePathInfo, Lazy<MarkdownLineInfo[]> markdownLines, 
        Lazy<byte[]> fileContents)
    {
        FilePathInfo = filePathInfo;
        _markdownLines = markdownLines;
        _fileContents = fileContents;
    }

    public static TodoFile Of(FilePathInfo filePathInfo, Lazy<MarkdownLineInfo[]> markdownLines, 
        Lazy<byte []> fileContents) 
        => new(filePathInfo, markdownLines, fileContents);
}
