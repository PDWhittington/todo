using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Data.Markdown;

public record TodoFile
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }

    private readonly Lazy<MarkdownLineInfo[]> _markdownLines;

    public MarkdownLineInfo[] MarkdownLines => _markdownLines.Value;

    private readonly Lazy<UnmanagedByteArray> _fileContents;

    public UnmanagedByteArray FileContents => _fileContents.Value;

    private TodoFile(FilePathInfo filePathInfo, Lazy<MarkdownLineInfo[]> markdownLines, 
        Lazy<UnmanagedByteArray> fileContents)
    {
        FilePathInfo = filePathInfo;
        _markdownLines = markdownLines;
        _fileContents = fileContents;
    }

    public static TodoFile Of(FilePathInfo filePathInfo, Lazy<MarkdownLineInfo[]> markdownLines, 
        Lazy<UnmanagedByteArray> fileContents) 
        => new(filePathInfo, markdownLines, fileContents);
}