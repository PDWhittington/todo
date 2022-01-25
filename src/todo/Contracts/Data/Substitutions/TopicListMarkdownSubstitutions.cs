namespace Todo.Contracts.Data.Substitutions;

public class TopicListMarkdownSubstitutions : MarkdownSubstitutionsBase
{
    public string TopicName { get; }

    private TopicListMarkdownSubstitutions(string topicName)
    {
        TopicName = topicName;
    }

    public static TopicListMarkdownSubstitutions Of(string topicName) => new(topicName);
}
