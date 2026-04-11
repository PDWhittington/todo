namespace Todo.Contracts.Data.Commands;

public record ShowWebpageCommand : CommandBase
{
    public static ShowWebpageCommand Singleton { get; } = new();

    private ShowWebpageCommand() { }
}
