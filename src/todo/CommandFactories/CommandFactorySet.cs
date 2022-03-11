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

    public IEnumerable<ICommandFactory<CommandBase>> GetAllCommandFactories()
    {
        return new[] { DefaultCommandFactory }
            .Concat(NonDefaultCommandFactories
                .OrderBy(x => x.CommandWords.First()))
            .ToArray();
    }

    public CommandFactorySet(IEnumerable<ICommandFactory<CommandBase>> commandFactories)
    {
        ValidateCommandFactories(commandFactories, out var defaultCommandFactory, out var nonDefaultCommandFactories);

        DefaultCommandFactory = defaultCommandFactory;
        NonDefaultCommandFactories = nonDefaultCommandFactories;
    }

    private static void ValidateCommandFactories(IEnumerable<ICommandFactory<CommandBase>> commandFactories,
        out ICommandFactory<CommandBase> defaultCommandFactory,
        out ICommandFactory<CommandBase> [] nonDefaultCommandFactories)
    {
        var exceptions = new List<Exception>();

        var commandFactoriesArr = commandFactories.ToArray();

        var wordsDuplicated = commandFactoriesArr
            .SelectMany(x => x.CommandWords)
            .GroupBy(x => x)
            .Where(g => g.Count() != 1)
            .Select(g => g.Key)
            .ToArray();

        if (wordsDuplicated.Any())
        {
            exceptions.Add(new Exception("The following words are taken by more " +
                $"than one CommandFactory: {string.Join(',',wordsDuplicated)}"));
        }

        var defaultCommandFactories = commandFactoriesArr
            .Where(x => x.IsDefaultCommandFactory)
            .ToArray();

        switch (defaultCommandFactories.Length)
        {
            case 0:
                exceptions.Add(new Exception("There are no CommandFactory objects marked as " +
                                             $"the default. {nameof(CommandFactoryBase<CommandBase>.IsDefaultCommandFactory)} " +
                                             "should be true for one and only one class"));
                break;
            case > 1:
                exceptions.Add(new Exception(
                    "The following CommandFactory objects are marked as being the default CommandFactory: " +
                    $"{string.Join(",", defaultCommandFactories.Select(x => x.GetType().Name))}. " +
                    $"{nameof(CommandFactoryBase<CommandBase>.IsDefaultCommandFactory)} " +
                    "should be true for one and only one class"));
                break;
        }

        if (exceptions.Any()) throw new AggregateException(
            "A number of exceptions were thrown while validating the CommandFactory objects", exceptions);

        defaultCommandFactory = commandFactoriesArr
            .Single(x => x.IsDefaultCommandFactory);

        nonDefaultCommandFactories = commandFactoriesArr
            .Where(x => !x.IsDefaultCommandFactory)
            .ToArray();
    }
}
