using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Reporting;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandFactory : CommandFactoryBase<SyncCommand>
{
    private static readonly string[] Words = { "s", "sync" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText => new[]
    {
        "Executes a commit and push operation sequentially.",
        "",
        "Usage: todo s [commit message]"
    };

    public SyncCommandFactory(IOutputWriter outputWriter) : base(outputWriter, Words) { }

    public override SyncCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? default : SyncCommand.Of(restOfCommand);
    }
}
