﻿using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandFactory : CommandFactoryBase<CommitCommand>
{
    private static readonly string[] Words = { "c", "commit" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } = {
        "Gathers the current modifications into a commit. Commit message is optional.",
        "",
        "Usage: todo c [commit message]"
    };

    public CommitCommandFactory(IOutputWriter outputWriter) : base(outputWriter, Words) { }

    public override CommitCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? default : CommitCommand.Of(restOfCommand);
    }
}
