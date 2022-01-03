using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.StateAndConfig;

public interface ICommandProvider
{
    public CommandBase GetCommand();
}
