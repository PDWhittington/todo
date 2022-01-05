﻿using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHelpCommandFactory : CommandFactoryBase<ShowHelpCommand>
{
    private static readonly string[] Words = { "help" };

    public override string[] HelpText => new[]
    {
        "Displays this help screen.",

        "Usage: todo help"
    };

    public ShowHelpCommandFactory() : base(Words) { }

    public override ShowHelpCommand? TryGetCommand(string commandLine)
    {
        return IsThisCommand(commandLine, out _)
            ? ShowHelpCommand.Singleton : default;
    }

    public override bool IsDefaultCommandFactory => false;
}