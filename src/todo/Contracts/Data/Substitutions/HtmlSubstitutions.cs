namespace Todo.Contracts.Data.Substitutions;

public class HtmlSubstitutions : SubstitutionsBase
{
    public string Title { get; }
    public string Body { get; }

    private HtmlSubstitutions(string title, string body)
    {
        Title = title;
        Body = body;
    }

    public static HtmlSubstitutions Of(string title, string body) => new(title, body);
}
