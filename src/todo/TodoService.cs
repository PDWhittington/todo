using System;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo;

public class TodoService : ITodoService
{
    private readonly ICommandProvider _commandProvider;
    private readonly ICommandExecutorSet _commandExecutorSet;

    public TodoService(ICommandProvider commandProvider, ICommandExecutorSet commandExecutorSet)
    {
        _commandProvider = commandProvider;
        _commandExecutorSet = commandExecutorSet;
    }

    public void PerformTask()
    {
        var command = _commandProvider.GetCommand();

        var commandExecutor = _commandExecutorSet.GetExecutorForCommand(command);

        if (commandExecutor == default) throw new Exception("Command not identified");

        commandExecutor.ExecuteCommandBase(command);
    }
}
