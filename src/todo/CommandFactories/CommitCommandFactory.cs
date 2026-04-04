using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandFactory(IOutputWriter outputWriter) 
    : CommandFactoryBase<CommitCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["c", "commit"];

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } =
    [
        "Gathers the current modifications into a commit. Commit message is optional.",
        "",
        "Usage: todo c [commit message]"
    ];

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override CommitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        return CommitCommand.Of(restOfCommand);
    }
}
