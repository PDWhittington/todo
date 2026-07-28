using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Todo;
using Todo.Contracts.Services;

Timer.Start();

var serviceProvider = Initialise.GetServiceProvider();

var service = serviceProvider.GetService<ITodoService>()!;

using var outputWriterHandle = service.InitialiseService();
var outputWriter = service.OutputWriter;

try
{
    service.PerformTask();
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
}
finally
{
    outputWriter.WriteLine();
    outputWriter.WriteLine($"App ran for {Timer.Elapsed.TotalMilliseconds} milliseconds.");
    await Log.CloseAndFlushAsync();
}