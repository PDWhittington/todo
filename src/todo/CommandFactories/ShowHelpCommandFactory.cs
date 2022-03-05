using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Reporting;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHelpCommandFactory : CommandFactoryBase<ShowHelpCommand>
{
    private static readonly string[] Words = { "help", "about" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Displays this help screen.",
        "",
        "Usage: todo help"
    };

    public ShowHelpCommandFactory(IOutputWriter outputWriter) : base(outputWriter, Words) { }

    public override ShowHelpCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException("Command expects nothing following.");

        return ShowHelpCommand.Singleton;
    }
}
