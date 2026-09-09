using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<KillHtmlCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["k", "killhtml"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Deletes all the html files in the todo folder and the archive subfolder."
    ];

    protected override string Usage => "k";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override KillHtmlCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        if (!string.IsNullOrWhiteSpace(commandLine.Switches))
            throw new ArgumentException("Command expects nothing following.");

        return KillHtmlCommand.Singleton;
    }
}
