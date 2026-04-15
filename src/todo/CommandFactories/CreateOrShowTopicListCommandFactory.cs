using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;


[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowTopicListCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<CreateOrShowTopicListCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["t", "topic"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Creates or shows a todo list relating to a single topic."
    ];

    protected override string Usage => "t (topic name)";

    public override CreateOrShowTopicListCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? null
            : CreateOrShowTopicListCommand.Of(restOfCommand?.Trim()
                ?? throw new Exception("Topic name cannot be blank"));
    }
}
