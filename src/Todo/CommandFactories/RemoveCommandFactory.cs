using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class RemoveCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
    : CommandFactoryBase<RemoveCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["rm", "remove", "delete"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    ["Deletes the file. If git is enabled, the command performs a remove in git."];

    protected override string Usage => "rm [date]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override RemoveCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand))
            return null;

        if (!dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in remove command is not recognised");

        return RemoveCommand.Of(dateOnly);
    }
}
