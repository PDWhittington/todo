using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.HelpMessages;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHelpCommandExecutor(
    IOutputWriter outputWriter,
    IConfigurationProvider configurationProvider,
    ICommandFactorySet commandFactorySet,
    IConsoleTextFormatter consoleTextFormatter,
    IBoilerPlateProvider boilerPlateProvider,
    ILogger<ShowHelpCommandExecutor> logger)
    : CommandExecutorBase<ShowHelpCommand>(outputWriter, logger), IShowHelpCommandExecutor
{
    public override void Execute(ShowHelpCommand command)
    {
        var commandHelpMessages = commandFactorySet
            .GetAllCommandFactories()
            .Select(cf => new { cf.CommandWords, HelpText = cf.GetFullHelpMessage().ToArray() })
            .Where(helpMessage => helpMessage.HelpText.Length != 0)
            .Select(cf =>
                new CommandHelpMessage(cf.CommandWords.ToArray(), cf.HelpText));

        var sb = new StringBuilder();

        boilerPlateProvider.MakeBoilerPlate(sb);

        sb
            .AppendLine("The following commands are available in this app:-")
            .AppendLine()
            .AppendLine(consoleTextFormatter.CreateTable(commandHelpMessages))
            .AppendLine();

        var notesLines = GetNotes();

        foreach (var notesLine in notesLines) sb.AppendLine(notesLine);

        OutputWriter.WriteLine(sb);
    }

    private IEnumerable<string> GetNotes()
    {
        var withSpecialChars = _notes
            .Select(x => x.Replace("->", "\u2192"));

        return consoleTextFormatter.WrapText(withSpecialChars,
            configurationProvider.ConfigInfo.Configuration.ConsoleWidth);
    }

    private readonly string [] _notes =
    [
        "Notes:",

        "",

        "createorshow is the default command. This means it can be accessed simply by typing anything that can be parsed as a date after the word todo.",

        "",

        "Valid date formats:-",

            "",

            "\t\"y\", \"yesterday\" -> yesterday",
            "\t(empty string), \".\", \"today\" -> today",
            "\t\"tm\", \"tomorrow\" -> tomorrow",

            "\t(day) -> the day/month/year which is nearest in time to today",
            "\t(day)/(month) -> the day/month which is nearest in time to today",
            "\t+(daycount) -> positive offset a number of days from today",
            "\t-(daycount) -> negative offset a number of days from today",

            "",

        "[Commit Message] -> In the Commit and Sync commands, the commit message is optional. If none is supplied, then a standard message detailing date and time of the commit will be used."
    ];
}