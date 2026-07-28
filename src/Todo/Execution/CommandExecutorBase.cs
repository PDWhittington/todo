using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

public abstract class CommandExecutorBase<T>(IOutputWriter outputWriter, ILogger<CommandExecutorBase<T>> logger) : ICommandExecutor<T>
    where T : CommandBase
{
    protected IOutputWriter OutputWriter { get; } = outputWriter;
    protected ILogger<CommandExecutorBase<T>> Logger { get; } = logger;
    
    public Type CommandType => typeof(T);

    public abstract void Execute(T command);

    public void ExecuteCommandBase(CommandBase command)
        => Execute(command as T ?? throw new Exception());
}