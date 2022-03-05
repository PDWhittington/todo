using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PushCommandFactory : CommandFactoryBase<PushCommand>
{
    private static readonly string[] Words = { "push" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Executes a git push.",
        "",
        "Usage: todo push"
    };

    public PushCommandFactory(IOutputWriter outputWriter) : base(outputWriter, Words) { }

    public override PushCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return PushCommand.Singleton;
    }
}
