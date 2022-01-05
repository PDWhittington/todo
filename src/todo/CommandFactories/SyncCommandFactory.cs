using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class SyncCommandFactory : CommandFactoryBase<SyncCommand>
{
    private static readonly string[] Words = { "sync", "s" };

    public override bool IsDefaultCommandFactory => false;

    public override string HelpText => "Executes a commit and push operation sequentially.";

    public SyncCommandFactory() : base(Words) { }

    public override SyncCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand)
            ? default : SyncCommand.Of(restOfCommand);
    }
}
