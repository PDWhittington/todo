namespace Todo.Contracts.Services.StateAndConfig;

public interface ICommandIdentifier
{
    public enum CommandTypeEnum
    {
        Archive,
        Sync
    }

    bool TryGetCommandType(out CommandTypeEnum? commandName, out string restOfCommand);
}