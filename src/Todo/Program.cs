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
    outputWriter.WriteLine(e.Message);
    outputWriter.WriteLine("Run 'todo help' for more information.");

    error = true;
}
catch (Exception e)
{
    outputWriter.WriteLine(
        $"The app threw the following exception:{Environment.NewLine}{Environment.NewLine}");
    outputWriter.WriteLine($"{e.GetType()}: {e.Message}");
    outputWriter.WriteLine();

    if (e.StackTrace is null)
    {
        outputWriter.WriteLine("Stack trace: <NULL>");
    }
    else
    {
        outputWriter.WriteLine("Stack trace:");
        outputWriter.WriteLine(e.StackTrace);
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
