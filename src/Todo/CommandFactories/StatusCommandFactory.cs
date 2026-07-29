using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class StatusCommandFactory(IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<StatusCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["status"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText => [];
    protected override string Usage => "";

    public override StatusCommand? TryGetCommand(string commandLine)
    {
        return !IsThisCommand(commandLine, out _) 
            ? null 
            : StatusCommand.Singleton;
    }
}
