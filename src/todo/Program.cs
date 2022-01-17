using System;
using Microsoft.Extensions.DependencyInjection;
using Todo;
using Todo.Contracts.Services;

var serviceProvider = Initialise.GetServiceProvider();

try
{
    serviceProvider
        .GetService<ITodoService>()!
        .PerformTask();
}
catch (Exception e)
{
    Console.WriteLine($"The app threw the following exception:{Environment.NewLine}{Environment.NewLine}" + e);
    throw;
}
