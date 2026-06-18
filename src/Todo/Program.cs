using System;
using Microsoft.Extensions.DependencyInjection;
using Todo;
using Todo.Contracts.Services;
using Todo.Contracts.Services.UI;

Timer.Start();

var serviceProvider = Initialise.GetServiceProvider();
var outputWriter = serviceProvider.GetService<IOutputWriter>()!;

try
{
    serviceProvider
        .GetService<ITodoService>()!
        .PerformTask();
}
catch (Exception e)
{
    outputWriter.WriteLine($"The app threw the following exception:{Environment.NewLine}{Environment.NewLine}");
    outputWriter.WriteLine($"{e.GetType()}: {e.Message}");
    outputWriter.WriteLine();

    if (e.StackTrace is not null)
    {
        outputWriter.WriteLine("Stack trace:");
        outputWriter.WriteLine(e.StackTrace);    
    }
}
finally
{
    outputWriter.WriteLine();
    outputWriter.JoinWritingThread();    
    
    Console.WriteLine($"App ran for {Timer.Elapsed.TotalMilliseconds} milliseconds.");
}

