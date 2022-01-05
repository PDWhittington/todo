using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateParsing;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ArchiveCommandFactory : CommandFactoryBase<ArchiveCommand>
{
    private static readonly string[] Words = { "a", "archive" };

    private readonly IDateParser _dateParser;

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText => new[]
    {
        "Archives the markdown file for a given date.",
        "Usage: todo a [date]"
    };

    public ArchiveCommandFactory(IDateParser dateParser)
        : base(Words)
    {
        _dateParser = dateParser;
    }

    public override ArchiveCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in archive command is not recognised");

        return ArchiveCommand.Of(dateOnly);
    }
}
