using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class CommandProvider : ICommandProvider
{
    private readonly ICommandLineProvider _commandLineProvider;
    private readonly ICommandFactorySet _commandFactorySet;


    public CommandProvider(ICommandLineProvider commandLineProvider,
        ICommandFactorySet commandFactorySet)
    {
        _commandLineProvider = commandLineProvider;
        _commandFactorySet = commandFactorySet;
    }

    public CommandBase GetCommand()
    {
        var commandLine = _commandLineProvider.GetCommandLineMinusAssemblyLocation();

        foreach (var commandFactory in _commandFactorySet.NonDefaultCommandFactories)
        {
            var command = commandFactory.TryGetCommand(commandLine);

            if (command != default) return command;
        }

        var commandForDefault = _commandFactorySet.DefaultCommandFactory.TryGetCommand(commandLine);

        if (commandForDefault != default) return commandForDefault;

        throw new Exception("Command not recognised");
    }
}
