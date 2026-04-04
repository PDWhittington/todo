using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandFactory(IConstantsProvider constantsProvider, IOutputWriter outputWriter)
    : CommandFactoryBase<InitCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["i", "init"];

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } =
    [
        $"Initialises the current folder with a default {constantsProvider.SettingsFileName} file. " +
        "If the todo and archive folders whose paths are specified in the settings file do not exist, " +
        "then these folders are created.",
        "",
        "Usage: todo init"
    ];

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override InitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return InitCommand.Singleton;
    }
}
