using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowDayListCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
    : CommandFactoryBase<CreateOrShowDayListCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["createorshow"];

    public override bool IsDefaultCommandFactory => true;

    protected override string[] HelpText { get; } =
    [
        "Creates or shows a markdown file for the date supplied. " +
        "This is the default command and can be executed by typing anything that can be parsed as a date. " +
        "Supplying no date assumes the current day."
    ];

    protected override string Usage => "[date]"; 

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override CreateOrShowDayListCommand TryGetCommand(string commandLine)
    {
        var commandLineToUse = IsThisCommand(commandLine, out var restOfCommand)
            ? restOfCommand : commandLine;

        if (!dateParser.TryGetDate(commandLineToUse, out var dateOnly))
        {
            throw new Exception("Date not recognised in command");
        }

        return CreateOrShowDayListCommand.Of(dateOnly);
    }
}
