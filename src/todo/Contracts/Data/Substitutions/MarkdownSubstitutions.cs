namespace Todo.Contracts.Data.Substitutions;

public class MarkdownSubstitutions : SubstitutionsBase
{
    public string DateText { get; }

    private MarkdownSubstitutions(string dateText)
    {
        DateText = dateText;
    }

    public static MarkdownSubstitutions Of(string dateText) => new(dateText);
}
