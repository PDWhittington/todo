using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;

namespace Todo.Execution;

public class ShowHelpExecutor : IShowHelpExecutor
{
    private readonly ICommandFactorySet _commandFactorySet;

    public ShowHelpExecutor(ICommandFactorySet commandFactorySet)
    {
        _commandFactorySet = commandFactorySet;
    }

    public void Execute(ShowHelpCommand command)
    {

    }
}
