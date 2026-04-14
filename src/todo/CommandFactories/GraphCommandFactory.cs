using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GraphCommandFactory(IOutputWriter outputWriter) 
    : CommandFactoryBase<GraphCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["g", "graph"];
    
    public override bool IsDefaultCommandFactory => false;
    
    public override string [] HelpText { get; } =
    [
        "Creates an HTML graph of your recent performance, using the same " +
        "logic as the score command."
    ];

    public override string Usage => "g";

    public override GraphCommand? TryGetCommand(string commandLine)
        => IsThisCommand(commandLine, out _)
            ? new GraphCommand()
            : null;
}

