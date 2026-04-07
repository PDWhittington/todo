namespace Todo.Contracts.Data.Substitutions;

public class HtmlSubstitutions : SubstitutionsBase
{
    public string Title { get; }
    public string Body { get; }
    
    public string Theme { get; }

    private HtmlSubstitutions(string title, string body, string theme)
    {
        Title = title;
        Body = body;
        Theme = theme;
    }

    public static HtmlSubstitutions Of(string title, string body, string theme) 
        => new(title, body, theme);
}
