using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<WhichTodoCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["w", "which"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Reveals the location of the todo executable. Running this command is "
            + "equivalent to invoking 'which todo' in bash, but can be run on any platform. "
            + "In fact, a 'boiler plate' of information is produced."
    ];

    protected override string Usage => "which";

    public override WhichTodoCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine))
            return null;

        return string.IsNullOrWhiteSpace(commandLine.Switches)
            ? WhichTodoCommand.Singleton
            : throw new ArgumentException("Command expects nothing following.");
    }
}
