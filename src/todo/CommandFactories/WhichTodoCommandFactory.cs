using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class WhichTodoCommandFactory : CommandFactoryBase<WhichTodoCommand>
{
    private static readonly string[] Words = { "which" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Reveals the location of the todo executable. Running this command is " +
        "equivalent to invoking 'which todo' in bash, but can be run on any platform. " +
        "In fact, a 'boiler plate' of information is produced.",
        "",
        "Usage: todo which"
    };

    public WhichTodoCommandFactory(IOutputWriter outputWriter)
        : base(outputWriter, Words) { }

    public override WhichTodoCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return WhichTodoCommand.Singleton;
    }
}
