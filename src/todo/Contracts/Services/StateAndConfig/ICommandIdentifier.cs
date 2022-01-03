namespace Todo.Contracts.Services.StateAndConfig;

public interface ICommandIdentifier
{
    public enum CommandTypeEnum
    {
        Archive,
        Commit,
        Sync,
        Push,
    }

    bool TryGetCommandType(out CommandTypeEnum? commandName, out string restOfCommand);
}