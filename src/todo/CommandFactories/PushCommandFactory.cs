using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;

namespace Todo.CommandFactories;

public class PushCommandFactory : CommandFactoryBase<PushCommand>
{
    public override bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "push"
        };

    public override PushCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out _)) return default;

        return PushCommand.Singleton;
    }
}
