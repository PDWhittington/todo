using System;
using Todo.Contracts.Exceptions;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo;

public class TodoService : ITodoService
{
    private readonly ICommandProvider _commandProvider;
    private readonly ICommandExecutorSet _commandExecutorSet;
    private readonly IOutputWriter _outputWriter;

    public TodoService(ICommandProvider commandProvider,
        ICommandExecutorSet commandExecutorSet, IOutputWriter outputWriter)
    {
        _commandProvider = commandProvider;
        _commandExecutorSet = commandExecutorSet;
        _outputWriter = outputWriter;
    }

    public void PerformTask()
    {
        using var handle = _outputWriter.CreateDisposableHandle();
        
        try
        {
            var command = _commandProvider.GetCommand();

            var commandExecutor = _commandExecutorSet.GetExecutorForCommand(command);

            if (commandExecutor == default) throw new Exception("Command not identified");

            commandExecutor.ExecuteCommandBase(command);
        }
        catch (TodoExceptionBase e)
        {
            _outputWriter.WriteLine($"An exception of type {e.GetType().Name} has been thrown:");
            _outputWriter.WriteLine(e.Advice());
        }
    }
}
