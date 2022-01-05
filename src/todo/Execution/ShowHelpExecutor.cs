using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.TextFormatting;

namespace Todo.Execution;

public class ShowHelpExecutor : CommandExecutorBase<ShowHelpCommand>, IShowHelpExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ICommandFactorySet _commandFactorySet;
    private readonly IConsoleTextFormatter _tableWriter;

    public ShowHelpExecutor(IConfigurationProvider configurationProvider, ICommandFactorySet commandFactorySet, IConsoleTextFormatter tableWriter)
    {
        _configurationProvider = configurationProvider;
        _commandFactorySet = commandFactorySet;
        _tableWriter = tableWriter;
    }

    public override void Execute(ShowHelpCommand command)
    {
        var commandHelpMessages = _commandFactorySet
            .GetAllCommandFactories()
            .Select(cf => new CommandHelpMessage(cf.CommandWords.ToArray(), cf.HelpText));

        var sb = new StringBuilder()
            .AppendLine(_tableWriter.CreateTable(commandHelpMessages))
            .AppendLine();

        var notesLines = GetNotes();

        foreach (var notesLine in notesLines) sb.AppendLine(notesLine);

        Console.WriteLine(sb);
    }

    private IEnumerable<string> GetNotes()
    {
        var withSpecialChars = _notes.Select(x => x.Replace("->", "\u2192"));

        return _tableWriter.WrapText(_notes, _configurationProvider.Config.ConsoleWidth);
    }

    private readonly string [] _notes =
    {
        "Notes:",

        "createorshow is the default command. This means it can be accessed simply by typing anything that can be parsed as a date after the word todo.",

        "Valid date formats:-",

        "\t\"y\", \"yesterday\" -> yesterday",
"\t[empty string], \".\", \"today\" -> today",
"\t\"tm\", \"tomorrow\" -> tomorrow",

"\t[day] -> maps to the day/month/year which is nearest in time to today",
"\t[day]/[month] -> maps to the day/month which is nearest in time to today",
"\t+[daycount] -> positive offset a number of days from today",
"\t-[daycount] -> negative offset a number of days from today",

        @"[Commit Message] -> In the Commit and Sync commands, the commit message is optional. If none is supplied, then 
a standard message detailing date and time of the commit will be used."
    };
}
