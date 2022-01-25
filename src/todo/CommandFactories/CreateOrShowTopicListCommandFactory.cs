using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;


[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowTopicListCommandFactory
    : CommandFactoryBase<CreateOrShowTopicListCommand>
{
    private static readonly string[] Words = { "t", "topic" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Creates or shows a todo list relating to a single topic.",
        "",
        "Usage: todo t (topic name)"
    };

    public CreateOrShowTopicListCommandFactory() : base(Words) { }

    public override CreateOrShowTopicListCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? default
            : CreateOrShowTopicListCommand.Of(restOfCommand?.Trim()
                ?? throw new Exception("Topic name cannot be blank"));
    }
}
