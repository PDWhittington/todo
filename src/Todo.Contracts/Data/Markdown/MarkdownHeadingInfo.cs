using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Data.Markdown;

public record MarkdownHeadingInfo
{
    public int HeadingLevel { get; }
    public ByteArraySpan HeadingTitle  { get; }

    private MarkdownHeadingInfo(int headingLevel, ByteArraySpan headingTitle)
    {
        HeadingLevel = headingLevel;
        HeadingTitle = headingTitle;
    }
    
    public static MarkdownHeadingInfo Of(int headingLevel, ByteArraySpan headingTitle)
        => new(headingLevel, headingTitle);
}