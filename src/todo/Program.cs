using Microsoft.Extensions.DependencyInjection;
using Todo.Contracts.Services;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;
using Todo.DateNaming;
using Todo.DateParsing;
using Todo.Execution;
using Todo.Git;
using Todo.Helpers;
using Todo.StateAndConfig;
using Todo.Template;

namespace Todo
{
    static class Program
    {
        static void Main()
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
                .AddStateAndConfig()
                .AddHelpers()
                .AddTemplateFunctionality()
                .AddDateNaming()
                .AddGitFunctionality()
                .AddMainExecutionLogic()
                .BuildServiceProvider();

            return serviceProvider;
        }
        
        static IServiceCollection AddStateAndConfig(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ICommandLineProvider, CommandLineProvider>()
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<ICommandProvider, CommandProvider>()
                .AddSingleton<ICommandIdentifier, CommandIdentifier>()
                .AddSingleton<IDateParser, DateParser>()
                .AddSingleton<ISettingsPathProvider, SettingsPathProvider>();
        
        static IServiceCollection AddHelpers(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IDateHelper, DateHelper>();

        static IServiceCollection AddTemplateFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ITemplateProvider, TemplateProvider>();
        
        static IServiceCollection AddDateNaming(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IChristmasNewYearDateNamer, ChristmasNewYearDateNamer>()
                .AddSingleton<IEasterDateNamer, EasterDateNamer>()
                .AddSingleton<ISaintsDayDateNamer, SaintsDayDateNamer>()
                .AddSingleton<IDateNamer, DateNamer>();

        static IServiceCollection AddGitFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGitInterface, GitInterface>();

        static IServiceCollection AddMainExecutionLogic(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ISyncExecutor, SyncExecutor>()
                .AddSingleton<ITodoService, TodoService>();

    }
}