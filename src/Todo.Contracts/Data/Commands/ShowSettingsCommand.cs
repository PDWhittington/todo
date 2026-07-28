namespace Todo.Contracts.Data.Commands;

public record ShowSettingsCommand : CommandBase
{
    public static ShowSettingsCommand Singleton { get; } = new();

    private ShowSettingsCommand() { }
}