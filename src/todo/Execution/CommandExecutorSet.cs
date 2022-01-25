using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;

namespace Todo.Execution;

public class CommandExecutorSet : ICommandExecutorSet
{
    private readonly Dictionary<Type, ICommandExecutor> _executors;

    public CommandExecutorSet(IEnumerable<ICommandExecutor> executors)
    {
        _executors = Validate(executors);
    }

    private static Dictionary<Type, ICommandExecutor> Validate(IEnumerable<ICommandExecutor> executors)
    {
        return executors
            .Where(_ => true)
            .ToDictionary(x => x.CommandType);
    }

    public ICommandExecutor? GetExecutorForCommand(CommandBase commandBase)
    {
        return _executors.TryGetValue(commandBase.GetType(), out var executor) ? executor : default;
    }

    public bool TryExecute<T>(T command) where T : CommandBase
    {
        if (_executors.TryGetValue(typeof(T), out var executor)) return false;

        executor!.ExecuteCommandBase(command);
        return true;
    }
}
