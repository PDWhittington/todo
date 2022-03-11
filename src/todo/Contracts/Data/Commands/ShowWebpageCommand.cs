namespace Todo.Contracts.Data.Commands;

public class ShowWebpageCommand : CommandBase
{
    public static ShowWebpageCommand Singleton { get; } = new();

    private ShowWebpageCommand() { }
}
