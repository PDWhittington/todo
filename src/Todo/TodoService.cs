using System;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Exceptions;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo;

public class TodoService(
    ICommandLineProvider commandLineProvider,
    IBoilerPlateProvider boilerPlateProvider,
    ICommandProvider commandProvider,
    ICommandExecutorSet commandExecutorSet,
    IOutputWriter outputWriter,
    ILogger<TodoService> logger)
    : ITodoService
{
    public void PerformTask()
    {
        using var handle = outputWriter.CreateDisposableHandle();

        logger.LogInformation("{Type}.{MethodName}: Starting Todo App. Command line: {commandLine}",
            GetType(), nameof(PerformTask), commandLineProvider.GetCommandLineMinusAssemblyLocation());

        logger.LogInformation("{Type}.{MethodName}: BuildInformation:{NewLine}{BoilerPlate}",
            GetType(), nameof(PerformTask), Environment.NewLine, 
            boilerPlateProvider.GetBoilerPlateForLogging());

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