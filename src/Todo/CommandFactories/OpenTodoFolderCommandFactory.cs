using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class OpenTodoFolderCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<OpenTodoFolderCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["explorer", "finder", "files"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Opens the todo folder in the system file manager (File Explorer on Windows, " +
        "Finder on macOS, or the default file manager on Linux)."
    ];

    protected override string Usage => "explorer";

    public override OpenTodoFolderCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        return !string.IsNullOrWhiteSpace(restOfCommand) 
            ? throw new ArgumentException("Command expects nothing following.") 
            : OpenTodoFolderCommand.Singleton;
    }
}