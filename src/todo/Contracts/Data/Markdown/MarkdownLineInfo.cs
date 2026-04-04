using System;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Data.Markdown;

public class MarkdownLineInfo
{
    public FilePathInfo FilePath { get; }
    public MarkdownLineTypeEnum LineType { get; }
    public string Line { get; }
    public double LineNumber { get; }
    public int HeadingLevel { get; }

    private MarkdownLineInfo(FilePathInfo filePathInfo, MarkdownLineTypeEnum lineType,
        string line, int lineNumber, int headingLevel)
    {
        FilePath = filePathInfo;
        LineType = lineType;
        Line = line;
        LineNumber = lineNumber;
        HeadingLevel = headingLevel;
    }
    
    public static MarkdownLineInfo Of(FilePathInfo filePathInfo, MarkdownLineTypeEnum lineType, 
        string line, int lineNumber, int headingLevel)
    {
        if (lineType != MarkdownLineTypeEnum.Heading && headingLevel != -1)
        {
            throw new ArgumentException($"should be set to -1 if {nameof(lineType)} " +
                                        $"is not set to {MarkdownLineTypeEnum.Heading}",  nameof(headingLevel));
        }
        
        return new MarkdownLineInfo(filePathInfo, lineType, line, lineNumber, headingLevel);
    }
}