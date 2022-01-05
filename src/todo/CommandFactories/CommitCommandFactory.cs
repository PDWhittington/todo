using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;

namespace Todo.CommandFactories;

public class CommitCommandFactory : CommandFactoryBase, ICommitCommandFactory
{
    public bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "commit",
            "c"
        };

    public CommitCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        return CommitCommand.Of(restOfCommand);
    }
}
