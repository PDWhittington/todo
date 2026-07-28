namespace Todo.Contracts.Data.Commands;

public record CreateOrShowTopicListCommand : CreateOrShowCommandBase
{
    public string Topic { get; }

    private CreateOrShowTopicListCommand(string topic)
    {
        Topic = topic;
    }

    public static CreateOrShowTopicListCommand Of(string topic) => new(topic);
}