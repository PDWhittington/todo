namespace Todo.Contracts.Data.Substitutions;

public class DayListMarkdownSubstitutions : MarkdownSubstitutionsBase
{
    public string DateText { get; }

    private DayListMarkdownSubstitutions(string dateText)
    {
        DateText = dateText;
    }

    public static DayListMarkdownSubstitutions Of(string dateText) => new(dateText);
}
