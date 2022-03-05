using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Reporting;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowConflictsCommandFactory : CommandFactoryBase<ShowConflictsCommand>
{
    private static readonly string[] Words = { "sc", "showconflicts" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } = {
        "Opens in the text editor all of the files for which conflicts exist ",
        "",
        "Usage: todo sc"
    };

    public ShowConflictsCommandFactory(IOutputWriter outputWriter)
        : base(outputWriter, Words)
    { }

    public override ShowConflictsCommand? TryGetCommand(string commandLine)
    {
        if(!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException($"{nameof(ShowConflictsCommand)} expects nothing following.");

        return ShowConflictsCommand.Singleton;
    }
}
