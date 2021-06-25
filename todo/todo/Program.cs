using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using todo.CommandLine;
using Todo.Contracts;
using Todo.Contracts.Services;
using Todo.Service;

namespace Todo
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();

            var todoService = serviceProvider.GetService<ITodoService>();
            todoService.PerformTask();
        }

        static ServiceProvider GetServiceProvider()
        {   
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ICommandLineParser, CommandLineParser>()
                .AddSingleton<ITodoService, TodoService>()
                .BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>();
                //.AddConsole(LogLevel.Debug);

            var logger = (serviceProvider.GetService<ILoggerFactory>() ?? throw new InvalidOperationException())
                .CreateLogger<Program>();
            
            logger.LogDebug("Starting application");

            return serviceProvider;
        }
    }
}