namespace Todo.Contracts.Data.Substitutions;

public record GraphHtmlSubstitutions : HtmlSubstitutions
{
    public string Title { get; }
    
    public string InitialTheme { get; }

    public string Svg { get; }
    
    public string Timestamp { get; }

    private GraphHtmlSubstitutions(string title, string initialTheme, string svg, string timestamp)
    {
        Title = title;
        InitialTheme = initialTheme;
        Svg = svg;
        Timestamp = timestamp;
    }

    public static GraphHtmlSubstitutions Of(string title, string initialTheme, string svg, string timestamp) 
        => new(title, initialTheme, svg, timestamp);
}