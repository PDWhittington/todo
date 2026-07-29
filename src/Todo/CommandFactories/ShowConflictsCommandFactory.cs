using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowConflictsCommandFactory(IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<ShowConflictsCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["sc", "showconflicts"];

    public override bool IsDefaultCommandFactory => false;

    protected override string [] HelpText { get; } =
    [
        "Opens in the text editor all of the files for which conflicts exist."
    ];

    protected override string Usage => "sc";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowConflictsCommand? TryGetCommand(string commandLine)
    {
        if(!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException($"{nameof(ShowConflictsCommand)} expects nothing following.");

        return ShowConflictsCommand.Singleton;
    }
}
