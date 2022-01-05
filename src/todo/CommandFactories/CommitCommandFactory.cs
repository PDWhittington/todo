using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

public class CommitCommandFactory : CommandFactoryBase<CommitCommand>
{
    public override bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "commit",
            "c"
        };

    public override CommitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        return CommitCommand.Of(restOfCommand);
    }
}
