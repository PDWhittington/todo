using System;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.TextFormatting;

namespace Todo.Execution;

public class ShowHelpExecutor : IShowHelpExecutor
{
    private readonly ICommandFactorySet _commandFactorySet;
    private readonly ITableWriter _tableWriter;

    public ShowHelpExecutor(ICommandFactorySet commandFactorySet, ITableWriter tableWriter)
    {
        _commandFactorySet = commandFactorySet;
        _tableWriter = tableWriter;
    }

    public void Execute(ShowHelpCommand command)
    {
        var commandHelpMessages = _commandFactorySet
            .GetAllCommandFactories()
            .Select(cf => new CommandHelpMessage(cf.CommandWords.ToArray(), cf.HelpText));

        var table = _tableWriter.CreateTable(commandHelpMessages);
        Console.WriteLine(table);
    }
}
