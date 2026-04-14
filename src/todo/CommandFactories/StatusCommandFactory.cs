using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class StatusCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<StatusCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["status"];
    
    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText => [];
    public override string Usage => "";

    public override StatusCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out var restOfCommand) 
            ? null 
            : StatusCommand.Singleton;
    }
}
