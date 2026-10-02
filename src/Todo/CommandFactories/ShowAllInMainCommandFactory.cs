using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowAllInMainCommandFactory(IOutputWriter outputWriter)
    : CommandFactoryBase<ShowAllInMainCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["all", "showallinmain"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "Opens every day todo list in the main folder, in date order."
    ];

    protected override string Usage => "all";

    public override ShowAllInMainCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        if (!string.IsNullOrWhiteSpace(commandLine.Switches))
            throw new ArgumentException("Command expects nothing following.");

        return new ShowAllInMainCommand();
    }
}
