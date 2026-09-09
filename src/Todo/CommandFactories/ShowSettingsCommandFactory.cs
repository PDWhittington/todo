using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandFactory(IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<ShowSettingsCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["config", "settings", "showsettings"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Shows the settings file in the default editor."
    ];

    protected override string Usage => "settings";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowSettingsCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        if (!string.IsNullOrWhiteSpace(commandLine.Switches))
            throw new ArgumentException("Command expects nothing following.");

        return ShowSettingsCommand.Singleton;
    }
}
