using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHtmlCommandFactory(IDateParser dateParser, IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<ShowHtmlCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["h", "html", "showhtml"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Opens the browser specified in the settings file and loads the Html file for the given date."
    ];

    protected override string Usage => "h [date]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in archive command is not recognised");

        return ShowHtmlCommand.Of(dateOnly);
    }
}
