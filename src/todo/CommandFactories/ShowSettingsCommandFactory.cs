using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<ShowSettingsCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["config", "settings", "showsettings"];

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } =
    [
        "Shows the settings file in the default editor.",
        "",
        "Usage: todo settings"
    ];

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowSettingsCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return ShowSettingsCommand.Singleton;
    }
}
