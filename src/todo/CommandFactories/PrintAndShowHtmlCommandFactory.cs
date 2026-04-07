using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintAndShowHtmlCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
    : CommandFactoryBase<PrintAndShowHtmlCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["ph", "printandshowhtml"];

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } =
    [
        "This command is equivalent to printhtml followed by showhtml (p, h).",
        "",
        "Usage: todo ph [date]"
    ];

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override PrintAndShowHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!dateParser.TryGetDate(restOfCommand, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintAndShowHtmlCommand.Of(dateOnly);
    }
}
