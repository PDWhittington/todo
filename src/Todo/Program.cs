using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Todo;
using Todo.Contracts.Exceptions;
using Todo.Contracts.Services;

Timer.Start();

var serviceProvider = Initialise.GetServiceProvider();
var service = serviceProvider.GetService<ITodoService>()!;

using var outputWriterHandle = service.InitialiseService();
var outputWriter = service.OutputWriter;
var consoleTextFormatter = service.ConsoleTextFormatter;
var error = false;

try
{
    service.PerformTask();
}
catch (CommandNotFoundException e)
{
    outputWriter.WriteLineAlways(e.Message);
    outputWriter.WriteLineAlways("Run 'todo help' for more information.");

    error = true;
}
catch (Exception e)
{
    outputWriter.WriteLineAlways(
        $"The app threw the following exception:{Environment.NewLine}{Environment.NewLine}");
    outputWriter.WriteLineAlways($"{e.GetType()}: {e.Message}");
    outputWriter.WriteLineAlways("");

    if (e.StackTrace is null)
    {
        outputWriter.WriteLineAlways("Stack trace: <NULL>");
    }
    else
    {
        outputWriter.WriteLineAlways("Stack trace:");
        outputWriter.WriteLineAlways(e.StackTrace);
    }

    error = true;
}
finally
{
    outputWriter.WriteLine();

    if (error)
    {
        outputWriter.WriteLine(consoleTextFormatter.FormatWithForegroundColour(
              $"App ran for {Timer.Elapsed.TotalMilliseconds} milliseconds.", ConsoleColor.Red));
    }
    else
    {
        outputWriter.WriteLine(consoleTextFormatter.FormatWithForegroundColour(
              $"App ran for {Timer.Elapsed.TotalMilliseconds} milliseconds.", ConsoleColor.Green));
    }

    await Log.CloseAndFlushAsync();
}
