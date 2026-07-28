using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class UnarchiveCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
    : CommandFactoryBase<UnarchiveCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["u", "unarchive"];

    public override bool IsDefaultCommandFactory => false;

    protected override string [] HelpText =>
    [
        "Un-archives the markdown file for a given date. The file is moved form the subfolder " +
        "back to the main todo folder. The name of the archive folder is specified in settings.json. Also in " +
        "settings.json can be specified whether the file is moved simply in the file system, or by using " +
        "git mv."
    ];

    protected override string Usage => "u [date]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override UnarchiveCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in un-archive command is not recognised");

        return UnarchiveCommand.Of(dateOnly);
    }
}