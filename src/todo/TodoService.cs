using System;
using Todo.Contracts.Exceptions;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo;

public class TodoService(
    ICommandProvider commandProvider,
    ICommandExecutorSet commandExecutorSet,
    IOutputWriter outputWriter)
    : ITodoService
{
    public void PerformTask()
    {
        using var handle = outputWriter.CreateDisposableHandle();
        
        try
        {
            var command = commandProvider.GetCommand();

            var commandExecutor = commandExecutorSet.GetExecutorForCommand(command);

            if (commandExecutor == null) throw new Exception("Command not identified");

            commandExecutor.ExecuteCommandBase(command);
        }
        catch (TodoExceptionBase e)
        {
            outputWriter.WriteLine($"An exception of type {e.GetType().Name} has been thrown:");
            outputWriter.WriteLine(e.Advice());
        }
    }
}
