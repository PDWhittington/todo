using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class OpenTodoFolderCommandFactory(IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<OpenTodoFolderCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["explorer", "finder", "files"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Opens the todo folder in the system file manager (File Explorer on Windows, " +
        "Finder on macOS, or the default file manager on Linux)."
    ];

    protected override string Usage => "explorer";

    public override OpenTodoFolderCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        return !string.IsNullOrWhiteSpace(commandLine.Switches) 
            ? throw new ArgumentException("Command expects nothing following.") 
            : OpenTodoFolderCommand.Singleton;
    }
}
