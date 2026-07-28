using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Todo.AppLaunching;
using Todo.AssemblyOperations;
using Todo.CommandFactories;
using Todo.Contracts.Services;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.GamifyOperations;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;
using Todo.Dates;
using Todo.Dates.Naming;
using Todo.Execution;
using Todo.FileSystem;
using Todo.FileSystem.Paths;
using Todo.GamifyOperations;
using Todo.Git;
using Todo.Git.Execution;
using Todo.MarkdownOperations;
using Todo.StateAndConfig;
using Todo.StringOperations;
using Todo.Templates;
using Todo.UI;

namespace Todo;

internal static class Initialise
{
    public static IServiceCollection GetServiceCollection() =>
        new ServiceCollection()
            /* Logging */
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // remove the default providers
                loggingBuilder.AddSerilog(dispose: true); // Serilog becomes the only provider
            })
            /* Base functionality */
            .AddAppLaunchingOperations()
            .AddAssemblyOperations()
            .AddStateAndConfig()
            .AddDateOperations()
            .AddTemplateFunctionality()
            .AddFileSystemFunctionality()
            .AddStringOperations()
            .AddGitFunctionality()
            .AddGamifyOperations()
            .AddMarkdownFunctionality()
            .AddUiFunctionality()
            /* Command interpretation and execution */
            /* These two methods are auto-generated in the SourceGenerators project */
            .RegisterCommandFactories()
            .RegisterCommandExecutors()
            .AddTypeSets()
            /* Main service */
            .AddTodoService();

    public static IServiceProvider GetServiceProvider()
    {
        var sessionId = Guid.NewGuid();

        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var todoPath = Path.Combine(homeFolder, ".todo.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.WithProperty("SessionId", sessionId) // identifies the session
            .WriteTo.Async(a =>
                a.File(
                    path: todoPath,
                    shared: true,
                    rollingInterval: RollingInterval.Infinite, // or Day if you prefer
                    buffered: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [Session:{SessionId}] {Message:lj}{NewLine}{Exception}"
                )
            )
            .CreateLogger();

        return GetServiceCollection().BuildServiceProvider();
    }

    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection RegisterSeveralInterfaces(
            Type[] interfaces,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
                Type implementation
        )
        {
            // Register the concrete implementation as singleton once
            serviceCollection.AddSingleton(implementation);

            foreach (var @interface in interfaces)
            {
                serviceCollection.AddSingleton(
                    @interface,
                    provider => provider.GetService(implementation)!
                );
            }

            return serviceCollection;
        }

        private IServiceCollection AddAppLaunchingOperations() =>
            serviceCollection
                .AddSingleton<IHtmlFileLauncher, HtmlFileLauncher>()
                .AddSingleton<ITextFileLauncher, TextFileLauncher>()
                .AddSingleton<IFileExplorerLauncher, FileExplorerLauncher>()
                .AddSingleton<ILaunchInfoSelector, LaunchInfoSelector>();

        private IServiceCollection AddAssemblyOperations() =>
            serviceCollection.AddSingleton<IManifestStreamProvider, ManifestStreamProvider>();

        private IServiceCollection AddStateAndConfig() =>
            serviceCollection
                .AddSingleton<IConstantsProvider, ConstantsProvider>()
                .AddSingleton<ICommandLineProvider, CommandLineProvider>()
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<ICommandProvider, CommandProvider>()
                .AddSingleton<ISettingsPathProvider, SettingsPathProvider>()
                .AddSingleton<IAssemblyInformationProvider, AssemblyInformationProvider>()
                .AddSingleton<IBoilerPlateProvider, BoilerPlateProvider>()
                .AddSingleton<IEnvironmentVariableProvider, EnvironmentVariableProvider>();

        private IServiceCollection AddDateOperations() =>
            serviceCollection
                .AddSingleton<IDateAccessor, DateAccessor>()
                .AddSingleton<IDateParser, DateParser>()
                .AddSingleton<IDateHelper, DateHelper>()
                .AddSingleton<IDateAdjuster, DateAdjuster>()
                .AddSingleton<IChristmasNewYearDateNamer, ChristmasNewYearDateNamer>()
                .AddSingleton<IEasterDateNamer, EasterDateNamer>()
                .AddSingleton<ISaintsDayDateNamer, SaintsDayDateNamer>()
                .AddSingleton<ISpecialDateNamer, SpecialDateNamer>()
                .AddSingleton<IDateFormatter, DateFormatter>()
                .AddSingleton<IFilenameDateParser, FilenameDateParser>()
                .AddSingleton<IOrdinalHelper, OrdinalHelper>();

        private IServiceCollection AddTemplateFunctionality() =>
            serviceCollection
                .AddSingleton<
                    IDayListMarkdownSubstitutionsMaker,
                    DayListMarkdownSubstitutionsMaker
                >()
                .AddSingleton<
                    ITopicListMarkdownSubstitutionsMaker,
                    TopicListMarkdownSubstitutionsMaker
                >()
                .AddSingleton<IListHtmlSubstitutionsMaker, ListHtmlSubstitutionsMaker>()
                .AddSingleton<IGraphHtmlSubstitutionsMaker, GraphHtmlSubstitutionsMaker>()
                .AddSingleton<IDayListMarkdownTemplateProvider, DayListMarkdownTemplateProvider>()
                .AddSingleton<
                    ITopicListMarkdownTemplateProvider,
                    TopicListMarkdownTemplateProvider
                >()
                .AddSingleton<IListHtmlTemplateProvider, ListHtmlTemplateProvider>()
                .AddSingleton<IGraphHtmlTemplateProvider, GraphHtmlTemplateProvider>();

        private IServiceCollection AddFileSystemFunctionality() =>
            serviceCollection
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IOutputFolderPathProvider, OutputFolderPathProvider>()
                .AddSingleton<IPathEnvironmentVariableRetriever, PathEnvironmentVariableRetriever>()
                .AddSingleton<IDateListPathResolver, DateListPathResolver>()
                .AddSingleton<ITopicListPathResolver, TopicListPathResolver>()
                .AddSingleton<IScoreHtmlPathResolver, ScoreHtmlPathResolver>()
                .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                .AddSingleton<IFileDeleter, FileDeleter>()
                .AddSingleton<IFolderCreator, FolderCreator>()
                .AddSingleton<IFileListCreator, FileListCreator>()
                .AddSingleton<IUnmanagedByteArrayManager, UnmanagedByteArrayManager>()
                .AddSingleton<IPinnedFileLoader, PinnedFileLoader>();

        private IServiceCollection AddStringOperations() =>
            serviceCollection.AddSingleton<IFastUtf8Substitutor, FastUtf8Substitutor>();

        private IServiceCollection AddGamifyOperations() =>
            serviceCollection.AddSingleton<IScoresGenerator, ScoresGenerator>();

        private IServiceCollection AddGitFunctionality() =>
            serviceCollection
                .AddSingleton<IGitInterface, GitInterface>()
                .AddSingleton<IGitCommandExecutorResolver, GitCommandExecutorResolver>()
                .AddSingleton<IGitAddCommandExecutor, GitAddCommandExecutor>()
                .AddSingleton<IGitCommitCommandExecutor, GitCommitCommandExecutor>()
                .AddSingleton<IGitGetConflictsCommandExecutor, GitGetConflictsCommandExecutor>()
                .AddSingleton<IGitGetRepoInfoCommandExecutor, GitGetRepoInfoCommandExecutor>()
                .AddSingleton<IGitMoveCommandExecutor, GitMoveCommandExecutor>()
                .AddSingleton<IGitPushCommandExecutor, GitPushCommandExecutor>()
                .AddSingleton<IGitRemoveCommandExecutor, GitRemoveCommandExecutor>()
                .AddSingleton<IGitResetCommandExecutor, GitResetCommandExecutor>();

        private IServiceCollection AddMarkdownFunctionality() =>
            serviceCollection
                .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                .AddSingleton<IMarkdownLineInterpreter, MarkdownLineInterpreter>();

        private IServiceCollection AddUiFunctionality() =>
            serviceCollection
                .AddSingleton<IConsoleTextFormatter, ConsoleTextFormatter>()
                .AddSingleton<IOutputWriter, OutputWriter>();

        private IServiceCollection AddTypeSets() =>
            serviceCollection
                .AddSingleton<ICommandFactorySet, CommandFactorySet>()
                .AddSingleton<ICommandExecutorSet, CommandExecutorSet>();

        private IServiceCollection AddTodoService() =>
            serviceCollection.AddSingleton<ITodoService, TodoService>();
    }
}