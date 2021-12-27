using Microsoft.Extensions.DependencyInjection;
using todo.CommandLine;
using Todo.Configuration;
using Todo.Contracts.Services;
using Todo.Service;
using Todo.State;
using Todo.Template;

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
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<IStateProvider, StateProvider>()
                .AddSingleton<ITemplateProvider, TemplateProvider>()
                .AddSingleton<ITodoService, TodoService>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}