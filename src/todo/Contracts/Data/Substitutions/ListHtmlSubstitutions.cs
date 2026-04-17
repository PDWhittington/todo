namespace Todo.Contracts.Data.Substitutions;

public record ListHtmlSubstitutions : HtmlSubstitutions
{
    public string Title { get; }

    public string Body { get; }
    
    public string Theme { get; }

    private ListHtmlSubstitutions(string title, string body, string theme)
    {
        Title = title;
        Body = body;
        Theme = theme;
    }

    public static ListHtmlSubstitutions Of(string title, string body, string theme) 
        => new(title, body, theme);
}
