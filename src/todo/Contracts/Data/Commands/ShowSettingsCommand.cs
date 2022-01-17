namespace Todo.Contracts.Data.Commands;

public class ShowSettingsCommand : CommandBase
{
    public static ShowSettingsCommand Singleton { get; } = new();

    private ShowSettingsCommand() { }
}
