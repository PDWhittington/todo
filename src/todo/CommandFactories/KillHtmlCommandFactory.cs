﻿using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandFactory : CommandFactoryBase<KillHtmlCommand>
{
    private static readonly string[] Words = { "k", "killhtml" };

    public KillHtmlCommandFactory() : base(Words) { }

    public override KillHtmlCommand? TryGetCommand(string commandLine)
        => !IsThisCommand(commandLine, out _)
            ? default : KillHtmlCommand.Singleton;

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Deletes all the html files in the todo folder and the archive subfolder",
        "Usage: todo k"
    };
}