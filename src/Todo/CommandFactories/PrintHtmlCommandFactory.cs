using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintHtmlCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
    : CommandFactoryBase<PrintHtmlCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["p", "print", "printhtml"];

    public override bool IsDefaultCommandFactory => false;

    protected override string [] HelpText { get; } =
    [
        "Converts a Markdown file to HTML. Can be used with anything that can be parsed as a date. " +
        "Supplying no date performs this operation on the Markdown file for the current day."
    ];

    protected override string Usage => "p [date]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override PrintHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;
        
        if (!dateParser.TryGetDate(restOfCommand, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintHtmlCommand.Of(dateOnly);
    }
}