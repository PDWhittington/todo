namespace Todo.Contracts.Data.Markdown;

public record MarkdownHeadingInfo
{
    public int HeadingLevel { get; }
    public string HeadingTitle  { get; }

    private MarkdownHeadingInfo(int headingLevel, string headingTitle)
    {
        HeadingLevel = headingLevel;
        HeadingTitle = headingTitle;
    }
    
    public static MarkdownHeadingInfo Of(int headingLevel, string headingTitle)
        => new(headingLevel, headingTitle);
}