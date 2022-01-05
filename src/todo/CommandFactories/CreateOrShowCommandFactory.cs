using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateParsing;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowCommandFactory : CommandFactoryBase<CreateOrShowCommand>
{
    private static readonly string[] Words = { "createorshow" };

    private readonly IDateParser _dateParser;

    public override bool IsDefaultCommandFactory => true;

    public override string HelpText =>
        "Creates or shows a markdown file for the date supplied. " +
        "This is the default command and can be executed by typing anything that can be parsed as a date. " +
        "Supplying no date assumes the current day";

    public CreateOrShowCommandFactory(IDateParser dateParser)
        : base(Words)
    {
        _dateParser = dateParser;
    }

    public override CreateOrShowCommand TryGetCommand(string commandLine)
    {
        var commandLineToUse = IsThisCommand(commandLine, out var restOfCommand)
            ? restOfCommand : commandLine;

        if (!_dateParser.TryGetDate(commandLineToUse, out var dateOnly))
        {
            throw new Exception("Date not recognised in command");
        }

        return CreateOrShowCommand.Of(dateOnly);
    }
}
