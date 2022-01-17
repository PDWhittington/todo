namespace Todo.Contracts.Data.Commands;

public class KillHtmlCommand : CommandBase
{
    public static KillHtmlCommand Singleton { get; } = new();

    private KillHtmlCommand() { }
}
