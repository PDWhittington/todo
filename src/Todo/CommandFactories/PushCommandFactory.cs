using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PushCommandFactory(IOutputWriter outputWriter) 
    : CommandFactoryBase<PushCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["push"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Executes a git push."
    ];

    protected override string Usage => "push";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override PushCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return PushCommand.Singleton;
    }
}