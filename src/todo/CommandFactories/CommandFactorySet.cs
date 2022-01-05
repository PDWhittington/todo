using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;

namespace Todo.CommandFactories;

public class CommandFactorySet : ICommandFactorySet
{
    public ICommandFactory<CommandBase> DefaultCommandFactory { get; }
    public ICommandFactory<CommandBase> [] NonDefaultCommandFactories { get; }

    public ICommandFactory<CommandBase>[] GetAllCommandFactories()
    {
        return new[] { DefaultCommandFactory }
            .Concat(NonDefaultCommandFactories)
            .ToArray();
    }


    public CommandFactorySet(IEnumerable<ICommandFactory<CommandBase>> commandFactories)
    {
        ValidateCommandFactories(commandFactories, out var defaultCommandFactory, out var nonDefaultCommandFactories);

        DefaultCommandFactory = defaultCommandFactory;
        NonDefaultCommandFactories = nonDefaultCommandFactories;
    }

    private void ValidateCommandFactories(IEnumerable<ICommandFactory<CommandBase>> commandFactories,
        out ICommandFactory<CommandBase> defaultCommandFactory,
        out ICommandFactory<CommandBase> [] nonDefaultCommandFactories)
    {
        var commandFactoriesArr = commandFactories.ToArray();

        var wordsDuplicated = commandFactoriesArr
            .SelectMany(x => x.CommandWords)
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
}
