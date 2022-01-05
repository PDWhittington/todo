using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Todo.CommandFactories;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.TextFormatting;
using Todo.DateNaming;
using Todo.DateParsing;
using Todo.Execution;
using Todo.FileSystem;
using Todo.Git;
using Todo.Helpers;
using Todo.StateAndConfig;
using Todo.Templates;
using Todo.TextFormatting;

namespace Todo;

internal static class Program
{
    private static void Main()
    {
        var serviceProvider = GetServiceProvider();

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
    }

    private static ServiceProvider GetServiceProvider()
        =>  new ServiceCollection()
            .AddLogging()
            .AddStateAndConfig()
            .AddHelpers()
            .AddTemplateFunctionality()
            .AddDateNaming()
            .AddFileSystemFunctionality()
            .AutoRegisterCommandFactories()
            .AddGitFunctionality()
            .AddHelpTextWritingFunctionality()
            .AddMainExecutionLogic()
            .BuildServiceProvider();

    private static IServiceCollection AutoRegisterCommandFactories(this IServiceCollection serviceCollection)
    {
        var commandFactories = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract)
            .Where(x => x.IsAssignableTo(typeof(ICommandFactory<CommandBase>)));

        foreach (var commandFactory in commandFactories)
        {
            serviceCollection.AddSingleton(commandFactory);
            serviceCollection.AddSingleton(typeof(ICommandFactory<CommandBase>), commandFactory);
        }

        serviceCollection.AddSingleton<ICommandFactorySet, CommandFactorySet>();

        return serviceCollection;
    }

    private static IServiceCollection AddStateAndConfig(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ICommandLineProvider, CommandLineProvider>()
            .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
            .AddSingleton<ICommandProvider, CommandProvider>()
            .AddSingleton<IDateParser, DateParser>()
            .AddSingleton<ISettingsPathProvider, SettingsPathProvider>();

    private static IServiceCollection AddHelpers(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IPathHelper, PathHelper>()
            .AddSingleton<IDateHelper, DateHelper>();

    private static IServiceCollection AddTemplateFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IMarkdownSubstitutionsMaker, MarkdownSubstitutionsMaker>()
            .AddSingleton<IHtmlSubstitutionsMaker, HtmlSubstitutionsMaker>()
            .AddSingleton<IMarkdownTemplateProvider, MarkdownTemplateProvider>()
            .AddSingleton<IHtmlTemplateProvider, HtmlTemplateProvider>();

    private static IServiceCollection AddDateNaming(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IChristmasNewYearDateNamer, ChristmasNewYearDateNamer>()
            .AddSingleton<IEasterDateNamer, EasterDateNamer>()
            .AddSingleton<ISaintsDayDateNamer, SaintsDayDateNamer>()
            .AddSingleton<ISpecialDateNamer, SpecialDateNamer>()
            .AddSingleton<IDateFormatter, DateFormatter>();

    private static IServiceCollection AddFileSystemFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IFileNamer, FileNamer>()
            .AddSingleton<IContentFilePathResolver, ContentFilePathResolver>()
            .AddSingleton<IFileReader, FileReader>()
            .AddSingleton<IMarkdownFileReader, MarkdownFileReader>();

    private static IServiceCollection AddGitFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IGitDependencyValidator, GitDependencyValidator>()
            .AddSingleton<IGitInterface, GitInterface>();

    private static IServiceCollection AddMainExecutionLogic(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IArchiveExecutor, ArchiveExecutor>()
            .AddSingleton<ICommitExecutor, CommitExecutor>()
            .AddSingleton<ICreateOrShowExecutor, CreateOrShowExecutor>()
            .AddSingleton<IPrintHtmlExecutor, PrintHtmlExecutor>()
            .AddSingleton<IPushExecutor, PushExecutor>()
            .AddSingleton<IShowHelpExecutor, ShowHelpExecutor>()
            .AddSingleton<IShowHtmlExecutor, ShowHtmlExecutor>()
            .AddSingleton<ISyncExecutor, SyncExecutor>()
            .AddSingleton<ITodoService, TodoService>();

    private static IServiceCollection AddHelpTextWritingFunctionality(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ITableWriter, TableWriter>();
}
