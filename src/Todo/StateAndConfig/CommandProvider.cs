using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Exceptions;

namespace Todo.StateAndConfig;

public class CommandProvider(
    ICommandLineProvider commandLineProvider,
    ICommandFactorySet commandFactorySet)
    : ICommandProvider
{
    public CommandBase GetCommand()
    {
        var commandLine = commandLineProvider.GetCommandLineMinusAssemblyLocation();

        foreach (var commandFactory in commandFactorySet.NonDefaultCommandFactories)
        {
            var command = commandFactory.TryGetCommand(commandLine);

            if (command != null) return command;
        }

        var commandForDefault = commandFactorySet.DefaultCommandFactory.TryGetCommand(commandLine);
        return commandForDefault ?? throw new CommandNotFoundException(commandLine);
    }
}
