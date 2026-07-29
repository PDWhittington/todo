using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandFactory(IConfigurationProvider configurationProvider, IConsoleTextFormatter consoleTextFormatter,
    IOutputWriter outputWriter) 
    : CommandFactoryBase<CommitCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["c", "commit"];

    public override bool IsDefaultCommandFactory => false;

    protected override string [] HelpText { get; } =
    [
        "Gathers the current modifications into a commit. Commit message is optional."
    ];

    protected override string Usage => "c [commit message]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override CommitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        return CommitCommand.Of(restOfCommand);
    }
}
