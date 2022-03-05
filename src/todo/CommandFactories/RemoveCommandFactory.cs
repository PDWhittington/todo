using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates.Parsing;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

public class RemoveCommandFactory : CommandFactoryBase<RemoveCommand>
{
    private static readonly string[] Words = { "rm", "remove", "delete" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Deletes the file. If git is enabled, the command performs a remove in git. ",
        "",
        "Usage: todo rm [date]"
    };

    private readonly IDateParser _dateParser;

    public RemoveCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
    {
        _dateParser = dateParser;
    }

    public override RemoveCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in remove command is not recognised");

        return RemoveCommand.Of(dateOnly);
    }
}
