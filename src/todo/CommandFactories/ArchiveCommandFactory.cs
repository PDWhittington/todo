using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ArchiveCommandFactory : CommandFactoryBase<ArchiveCommand>
{
    private static readonly string[] Words = { "a", "archive" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } = {
        "Archives the markdown file for a given date. The file is moved into the archive subfolder " +
        "of the main todo folder. The name of the archive folder is specified in settings.json. Also in " +
        "settings.json can be specified whether the file is moved simply in the file system, or by using " +
        "git mv.",
        "",
        "Usage: todo a [date]"
    };

    private readonly IDateParser _dateParser;

    public ArchiveCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
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
