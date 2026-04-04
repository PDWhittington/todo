using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<WhichTodoCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["which"];

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } =
    [
        "Reveals the location of the todo executable. Running this command is " +
        "equivalent to invoking 'which todo' in bash, but can be run on any platform. " +
        "In fact, a 'boiler plate' of information is produced.",
        "",
        "Usage: todo which"
    ];

    public override WhichTodoCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        return string.IsNullOrWhiteSpace(restOfCommand)
            ? WhichTodoCommand.Singleton
            : throw new ArgumentException("Command expects nothing following.");
    }
}
