namespace Todo.Contracts.Data.Substitutions;

public record GraphHtmlSubstitutions : HtmlSubstitutions
{
    public string Title { get; }

    public string Svg { get; }
    
    public string Timestamp { get; }

    private GraphHtmlSubstitutions(string title, string svg, string timestamp)
    {
        Title = title;
        Svg = svg;
        Timestamp = timestamp;
    }

    public static GraphHtmlSubstitutions Of(string title, string svg, string timestamp) 
        => new(title, svg, timestamp);
}