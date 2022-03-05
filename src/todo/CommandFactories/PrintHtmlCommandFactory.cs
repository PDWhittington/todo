using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates.Parsing;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintHtmlCommandFactory : CommandFactoryBase<PrintHtmlCommand>
{
    private static readonly string[] Words = { "p", "print", "printhtml" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } = {
        "Converts a Markdown file to HTML. Can be used with anything that can be parsed as a date. " +
        "Supplying no date performs this operation on the Markdown file for the current day.",
        "",
        "Usage: todo p [date]"
    };

    private readonly IDateParser _dateParser;

    public PrintHtmlCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
    {
        _dateParser = dateParser;
    }

    public override PrintHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintHtmlCommand.Of(dateOnly);
    }
}
