using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandFactory: CommandFactoryBase<InitCommand>
{
    private static readonly string[] Words = { "i", "init" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; }

    public InitCommandFactory(IConstantsProvider constantsProvider, IOutputWriter outputWriter) : base(outputWriter, Words)
    {
        HelpText = new [] {
            $"Initialises the current folder with a default {constantsProvider.SettingsFileName} file",
            "",
            "Usage: todo init"
        };
    }

    public override InitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return InitCommand.Singleton;
    }
}
