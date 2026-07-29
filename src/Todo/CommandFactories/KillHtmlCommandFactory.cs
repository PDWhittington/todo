using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandFactory(IConfigurationProvider configurationProvider,
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<KillHtmlCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["k", "killhtml"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Deletes all the html files in the todo folder and the archive subfolder."
    ];

    protected override string Usage => "k";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override KillHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return KillHtmlCommand.Singleton;
    }
}
