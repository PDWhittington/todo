using Microsoft.Extensions.DependencyInjection;
using Todo.Contracts.Services;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;
using Todo.DateNaming;
using Todo.DateParsing;
using Todo.Execution;
using Todo.FileNaming;
using Todo.Git;
using Todo.Helpers;
using Todo.StateAndConfig;
using Todo.Template;

namespace Todo;

internal static class Program
{
    private static void Main()
    {
        var serviceProvider = GetServiceProvider();

        serviceProvider
            .GetService<ITodoService>()!
            .PerformTask();
    }

    private static ServiceProvider GetServiceProvider()
        =>  new ServiceCollection()
            .AddLogging()
            .AddStateAndConfig()
            .AddHelpers()
            .AddTemplateFunctionality()
            .AddDateNaming()
            .AddFileNaming()
            .AddGitFunctionality()
            .AddMainExecutionLogic()
            .BuildServiceProvider();

    private static IServiceCollection AddStateAndConfig(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ICommandLineProvider, CommandLineProvider>()
            .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
            .AddSingleton<ICommandProvider, CommandProvider>()
            .AddSingleton<ICommandIdentifier, CommandIdentifier>()
            .AddSingleton<IDateParser, DateParser>()
            .AddSingleton<ISettingsPathProvider, SettingsPathProvider>();

    private static IServiceCollection AddHelpers(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IPathHelper, PathHelper>()
            .AddSingleton<IDateHelper, DateHelper>();

    private static IServiceCollection AddTemplateFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ISubstitutionMaker, SubstitutionMaker>()
            .AddSingleton<ITemplateProvider, TemplateProvider>();

    private static IServiceCollection AddDateNaming(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IChristmasNewYearDateNamer, ChristmasNewYearDateNamer>()
            .AddSingleton<IEasterDateNamer, EasterDateNamer>()
            .AddSingleton<ISaintsDayDateNamer, SaintsDayDateNamer>()
            .AddSingleton<ISpecialDateNamer, DateNamer>();

    private static IServiceCollection AddFileNaming(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IFileNamer, FileNamer>();

    private static IServiceCollection AddGitFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IGitDependencyValidator, GitDependencyValidator>()
            .AddSingleton<IGitInterface, GitInterface>();

    private static IServiceCollection AddMainExecutionLogic(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IArchiveExecutor, ArchiveExecutor>()
            .AddSingleton<ICommitExecutor, CommitExecutor>()
            .AddSingleton<ICreateOrShowExecutor, CreateOrShowExecutor>()
            .AddSingleton<IPushExecutor, PushExecutor>()
            .AddSingleton<IShowHtmlExecutor, ShowHtmlExecutor>()
            .AddSingleton<ISyncExecutor, SyncExecutor>()
            .AddSingleton<ITodoService, TodoService>();
}
