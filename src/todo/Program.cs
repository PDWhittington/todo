using System;
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
using Todo.DateNaming;
using Todo.DateParsing;
using Todo.Execution;
using Todo.FileSystem;
using Todo.Git;
using Todo.Helpers;
using Todo.StateAndConfig;
using Todo.Templates;

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
            .AddCommandFactories()
            .AddGitFunctionality()
            .AddMainExecutionLogic()
            .BuildServiceProvider();

    private static IServiceCollection AddCommandFactories(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IArchiveCommandFactory, ArchiveCommandFactory>()
            .AddSingleton<ICommitCommandFactory, CommitCommandFactory>()
            .AddSingleton<ICreateOrShowCommandFactory, CreateOrShowCommandFactory>()
            .AddSingleton<IPrintHtmlCommandFactory, PrintHtmlCommandFactory>()
            .AddSingleton<IPushCommandFactory, PushCommandFactory>()
            .AddSingleton<IShowHtmlCommandFactory, ShowHtmlCommandFactory>()
            .AddSingleton<ISyncCommandFactory, SyncCommandFactory>()

            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<IArchiveCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<ICommitCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<ICreateOrShowCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<IPrintHtmlCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<IPushCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<IShowHtmlCommandFactory>())
            .AddSingleton<ICommandFactory<CommandBase>>(x => x.GetRequiredService<ISyncCommandFactory>());

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
            .AddSingleton<IContentFileResolver, ContentFileResolver>()
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
            .AddSingleton<IShowHtmlExecutor, ShowHtmlExecutor>()
            .AddSingleton<ISyncExecutor, SyncExecutor>()
            .AddSingleton<ITodoService, TodoService>();
}
