using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandFactory : CommandFactoryBase<ShowSettingsCommand>
{
    private static readonly string[] Words = { "settings", "showsettings" };

    public ShowSettingsCommandFactory() : base(Words)
    { }

    public override ShowSettingsCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return ShowSettingsCommand.Singleton;
    }

    public override bool IsDefaultCommandFactory { get; }
    public override string[] HelpText { get; } = {
        "Shows the settings file in the default editor.",

        "Usage: todo settings"
    };
}
