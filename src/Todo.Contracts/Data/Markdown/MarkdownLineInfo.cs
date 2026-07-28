using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Data.Markdown;

public record MarkdownLineInfo
{
    public MarkdownLineTypeEnum LineType { get; }
    
    public ByteArraySpan Line { get; }
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public double LineNumber { get; }
    public int HeadingLevel { get; }

    private MarkdownLineInfo(MarkdownLineTypeEnum lineType, ByteArraySpan line, 
        int lineNumber, int headingLevel)
    {
        LineType = lineType;
        Line = line;
        LineNumber = lineNumber;
        HeadingLevel = headingLevel;
    }

    // public override string ToString() => Line;
    
    public static MarkdownLineInfo Of(MarkdownLineTypeEnum lineType, 
        ByteArraySpan line, int lineNumber, int headingLevel)
    {
        if (lineType != MarkdownLineTypeEnum.Heading && headingLevel != -1)
        {
            throw new ArgumentException($"should be set to -1 if {nameof(lineType)} " +
                                        $"is not set to {MarkdownLineTypeEnum.Heading}",  nameof(headingLevel));
        }
        
        return new MarkdownLineInfo(lineType, line, lineNumber, headingLevel);
    }
}