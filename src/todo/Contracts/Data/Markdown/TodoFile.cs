using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Markdown;

public class TodoFile
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }
    
    public ReadOnlyCollection<string> Lines { get; }
    
    private Lazy<MarkdownLineInfo[]> _markdownLines;

    public MarkdownLineInfo[] MarkdownLines => _markdownLines.Value;

    private Lazy<string> _fileContents;
    
    public string FileContents => _fileContents.Value;
    
    // ReSharper disable once MemberCanBePrivate.Global
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
