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

    private readonly ICommandFactory<CommandBase> _defaultCommandFactory;
    private readonly ICommandFactory<CommandBase> [] _nonDefaultCommandFactories;

    public CommandProvider(ICommandLineProvider commandLineProvider,
        IEnumerable<ICommandFactory<CommandBase>> commandFactories)
    {
        _commandLineProvider = commandLineProvider;

        ValidateCommandFactories(commandFactories, out _defaultCommandFactory, out _nonDefaultCommandFactories);
    }

    private void ValidateCommandFactories(IEnumerable<ICommandFactory<CommandBase>> commandFactories,
        out ICommandFactory<CommandBase> defaultCommandFactory, out ICommandFactory<CommandBase>[] nonDefaultCommandFactories)
    {
        var commandFactoriesArr = commandFactories.ToArray();

        var wordsDuplicated = commandFactoriesArr
            .SelectMany(x => x.WordsForCommand)
            .GroupBy(x => x)
            .Where(g => g.Count() != 1)
            .Select(g => g.Key)
            .ToArray();

        if (wordsDuplicated.Any())
        {
            throw new Exception(
                $"The following words are taken by more than one CommandFactory: {string.Join(',',wordsDuplicated)}");
        }


        var oneAndOnlyOneDefault = commandFactoriesArr
            .Count(x => x.IsDefaultCommandFactory) == 1;

        if (!oneAndOnlyOneDefault)
        {
            throw new Exception("There should be one and only one CommandFactory which is the default");
        }

        defaultCommandFactory = commandFactoriesArr
            .Single(x => x.IsDefaultCommandFactory);

        nonDefaultCommandFactories = commandFactoriesArr
            .Where(x => !x.IsDefaultCommandFactory)
            .ToArray();
    }

    public CommandBase GetCommand()
    {
        var commandLine = _commandLineProvider.GetCommandLineMinusAssemblyLocation();

        foreach (var commandFactory in _nonDefaultCommandFactories)
        {
            var command = commandFactory.TryGetCommand(commandLine);

            if (command != default) return command;
        }

        var commandForDefault = _defaultCommandFactory.TryGetCommand(commandLine);

        if (commandForDefault != default) return commandForDefault;

        throw new Exception("Command not recognised");
    }
}
