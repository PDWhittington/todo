using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandFactory(IOutputWriter outputWriter) 
    : CommandFactoryBase<SyncCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["s", "sync"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Executes a commit and push operation sequentially."
    ];

    protected override string Usage => "s [commit message]";

    public override SyncCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? null : SyncCommand.Of(restOfCommand);
    }
}