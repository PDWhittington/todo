using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHelpCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<ShowHelpCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["help", "about"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Displays this help screen."
    ];

    protected override string Usage => "help";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowHelpCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return ShowHelpCommand.Singleton;
    }
}
