using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.Reporting;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintAndShowHtmlCommandFactory : CommandFactoryBase<PrintAndShowHtmlCommand>
{
    private static readonly string[] Words = { "ph", "printandshowhtml" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } =
    {
        "This command is equivalent to printhtml followed by showhtml (p, h).",
        "",
        "Usage: todo ph [date]"
    };

    private readonly IDateParser _dateParser;

    public PrintAndShowHtmlCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
    {
        _dateParser = dateParser;
    }

    public override PrintAndShowHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintAndShowHtmlCommand.Of(dateOnly);
    }
}
