using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;

namespace Todo.Execution;

public abstract class CommandExecutorBase<T> : ICommandExecutor<T>
    where T : CommandBase
{
    public Type CommandType => typeof(T);

    public abstract void Execute(T command);

    public void ExecuteCommandBase(CommandBase command)
        => Execute(command as T ?? throw new Exception());
}
