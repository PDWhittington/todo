using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHelpCommandExecutor : CommandExecutorBase<ShowHelpCommand>, IShowHelpCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ICommandFactorySet _commandFactorySet;
    private readonly IConsoleTextFormatter _consoleTextFormatter;
    private readonly IVersionProvider _versionProvider;

    public ShowHelpCommandExecutor(IOutputWriter outputWriter,
        IConfigurationProvider configurationProvider, ICommandFactorySet commandFactorySet,
        IConsoleTextFormatter consoleTextFormatter, IVersionProvider versionProvider)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _commandFactorySet = commandFactorySet;
        _consoleTextFormatter = consoleTextFormatter;
        _versionProvider = versionProvider;
    }

    public override void Execute(ShowHelpCommand command)
    {
        var commandHelpMessages = _commandFactorySet
            .GetAllCommandFactories()
            .Select(cf =>
                new CommandHelpMessage(cf.CommandWords.ToArray(), cf.HelpText));

        var sb = new StringBuilder()
            .AppendLine($"Todo version (commit): {_versionProvider.GetVersion()}")
            .AppendLine($"Framework version: {RuntimeInformation.FrameworkDescription}")
            .AppendLine($"Process architecture: {RuntimeInformation.ProcessArchitecture}")
            .AppendLine($"OS architecture: {RuntimeInformation.OSArchitecture}")
            .AppendLine($"OS description: {RuntimeInformation.OSDescription}")
            .AppendLine()
            .AppendLine("The following commands are available in this app:-")
            .AppendLine()
            .AppendLine(_consoleTextFormatter.CreateTable(commandHelpMessages))
            .AppendLine();

        var notesLines = GetNotes();

        foreach (var notesLine in notesLines) sb.AppendLine(notesLine);

        OutputWriter.WriteLine(sb);
    }

    private IEnumerable<string> GetNotes()
    {
        var withSpecialChars = _notes
            .Select(x => x.Replace("->", "\u2192"));

        return _consoleTextFormatter.WrapText(withSpecialChars, _configurationProvider.Config.ConsoleWidth);
    }

    private readonly string [] _notes =
    {
        "Notes:",

        "",

        "createorshow is the default command. This means it can be accessed simply by typing anything that can be parsed as a date after the word todo.",

        "",

        "Valid date formats:-",

            "",

            "\t\"y\", \"yesterday\" -> yesterday",
            "\t[empty string], \".\", \"today\" -> today",
            "\t\"tm\", \"tomorrow\" -> tomorrow",

            "\t[day] -> maps to the day/month/year which is nearest in time to today",
            "\t[day]/[month] -> maps to the day/month which is nearest in time to today",
            "\t+[daycount] -> positive offset a number of days from today",
            "\t-[daycount] -> negative offset a number of days from today",

            "",

        @"[Commit Message] -> In the Commit and Sync commands, the commit message is optional. If none is supplied, then a standard message detailing date and time of the commit will be used."
    };
}
