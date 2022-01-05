using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;

namespace Todo.CommandFactories;

public class SyncCommandFactory : CommandFactoryBase<SyncCommand>
{
    public override bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.CurrentCultureIgnoreCase)
        {
            "sync",
            "s"
        };

    public override SyncCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        return SyncCommand.Of(restOfCommand);
    }
}
