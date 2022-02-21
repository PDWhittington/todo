using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandFactory: CommandFactoryBase<InitCommand>
{
    private readonly IConstantsProvider _constantsProvider;
    private static readonly string[] Words = { "i", "init" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; }

    public InitCommandFactory(IConstantsProvider constantsProvider) : base(Words)
    {
        _constantsProvider = constantsProvider;

        HelpText = new [] {
            $"Initialises the current folder with a default {_constantsProvider.SettingsFileName} file",
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
