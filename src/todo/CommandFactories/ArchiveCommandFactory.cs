using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.DateParsing;

namespace Todo.CommandFactories;

public class ArchiveCommandFactory : CommandFactoryBase, IArchiveCommandFactory
{
    private readonly IDateParser _dateParser;

    public bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; } = new(StringComparer.InvariantCultureIgnoreCase)
    {
        "a", "archive"
    };

    public ArchiveCommandFactory(IDateParser dateParser)
    {
        _dateParser = dateParser;
    }

    public ArchiveCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in archive command is not recognised");

        return ArchiveCommand.Of(dateOnly);
    }
}
