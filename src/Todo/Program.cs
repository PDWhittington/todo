using System;
using Microsoft.Extensions.DependencyInjection;
using Todo;
using Todo.Contracts.Services;

var serviceProvider = Initialise.GetServiceProvider();

try
{
    Timer.Start();

    serviceProvider
        .GetService<ITodoService>()!
        .PerformTask();
}
catch (Exception e)
{
    Console.WriteLine($"The app threw the following exception:{Environment.NewLine}{Environment.NewLine}");
    Console.WriteLine($"{e.GetType()}: {e.Message}");
}
finally
{
    Console.WriteLine();
    Console.WriteLine($"App ran for {Timer.Elapsed.TotalMilliseconds} milliseconds.");
}
