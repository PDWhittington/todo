using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
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
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.StringOperations;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;
using Todo.Dates;
using Todo.Dates.Naming;
using Todo.DependencyInjection;
using Todo.Execution;
using Todo.FileSystem;
using Todo.FileSystem.Paths;
using Todo.GamifyOperations;
using Todo.Git;
using Todo.MarkdownOperations;
using Todo.StateAndConfig;
using Todo.StringOperations;
using Todo.Templates;
using Todo.UI;

namespace Todo;

internal static class Initialise
{
    public static IServiceCollection GetServiceCollection()
        => new ServiceCollection()
            .AddLogging()

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
            .AutoRegisterCommandFactories()
            .AutoRegisterCommandExecutors()
            .AutoRegisterExecutorInterfaces()
            .AddTypeSets()

            /* Main service */
            .AddTodoService();

    public static IServiceProvider GetServiceProvider()
        => GetServiceCollection()

            /* Build the service provider */
            .BuildServiceProvider();
    
    #region Base functionality

    extension(IServiceCollection serviceCollection)
    {
        private IServiceCollection AddAppLaunchingOperations()
            => serviceCollection
                .AddSingleton<IHtmlFileLauncher, HtmlFileLauncher>()
                .AddSingleton<ITextFileLauncher, TextFileLauncher>();

        private IServiceCollection AddAssemblyOperations()
            => serviceCollection
                .AddSingleton<IManifestStreamProvider, ManifestStreamProvider>();

        private IServiceCollection AddStateAndConfig()
            => serviceCollection
                .AddSingleton<IConstantsProvider, ConstantsProvider>()
                .AddSingleton<ICommandLineProvider, CommandLineProvider>()
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<ICommandProvider, CommandProvider>()
                .AddSingleton<ISettingsPathProvider, SettingsPathProvider>()
                .AddSingleton<IAssemblyInformationProvider, AssemblyInformationProvider>()
                .AddSingleton<IBoilerPlateProvider, BoilerPlateProvider>();

        private IServiceCollection AddDateOperations()
            => serviceCollection
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

        private IServiceCollection AddTemplateFunctionality()
            => serviceCollection
                .AddSingleton<IDayListMarkdownSubstitutionsMaker, DayListMarkdownSubstitutionsMaker>()
                .AddSingleton<ITopicListMarkdownSubstitutionsMaker, TopicListMarkdownSubstitutionsMaker>()
                .AddSingleton<IListHtmlSubstitutionsMaker, ListHtmlSubstitutionsMaker>()
                .AddSingleton<IGraphHtmlSubstitutionsMaker, GraphHtmlSubstitutionsMaker>()
                .AddSingleton<IDayListMarkdownTemplateProvider, DayListMarkdownTemplateProvider>()
                .AddSingleton<ITopicListMarkdownTemplateProvider, TopicListMarkdownTemplateProvider>()
                .AddSingleton<IListHtmlTemplateProvider, ListHtmlTemplateProvider>()
                .AddSingleton<IGraphHtmlTemplateProvider, GraphHtmlTemplateProvider>();

        private IServiceCollection AddFileSystemFunctionality()
            => serviceCollection
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

        private IServiceCollection AddStringOperations()
            => serviceCollection
                .AddSingleton<IFastUtf8Substitutor, FastUtf8Substitutor>();

        private IServiceCollection AddGamifyOperations()
            => serviceCollection.AddSingleton<IScoresGenerator, ScoresGenerator>();

        private IServiceCollection AddGitFunctionality()
            => serviceCollection
                .AddSingleton<IGitInterface, GitInterface>()
                .AddSingleton<IGitInterfaceTools, GitInterfaceTools>();

        private IServiceCollection AddMarkdownFunctionality()
            => serviceCollection
                .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                .AddSingleton<IMarkdownLineInterpreter, MarkdownLineInterpreter>();

        private IServiceCollection AddUiFunctionality()
            => serviceCollection
                .AddSingleton<IConsoleTextFormatter, ConsoleTextFormatter>()
                .AddSingleton<IOutputWriter, OutputWriter>();
    }

    #endregion

    extension(IServiceCollection serviceCollection)
    {
        private IServiceCollection AutoRegisterCommandFactories()
        {
            return serviceCollection.AddCommandFactories();
        }

        private IServiceCollection AutoRegisterCommandExecutors()
        {
            throw new NotImplementedException();
            // return serviceCollection.AddCommandFactoriesExecutors();
        }

        private IServiceCollection AutoRegisterExecutorInterfaces()
        {
            return serviceCollection
                .AddSingleton<IArchiveCommandExecutor, ArchiveCommandExecutor>()
                .AddSingleton<ICommitCommandExecutor, CommitCommandExecutor>()
                .AddSingleton<ICreateOrShowDayListCommandExecutor, CreateOrShowDayListCommandExecutor>()
                .AddSingleton<ICreateOrShowTopicListCommandExecutor, CreateOrShowTopicListCommandExecutor>()
                .AddSingleton<IGraphCommandExecutor, GraphCommandExecutor>()
                .AddSingleton<IInitCommandExecutor, InitCommandExecutor>()
                .AddSingleton<IKillHtmlCommandExecutor, KillHtmlCommandExecutor>()
                .AddSingleton<IListFilesCommandExecutor, ListFilesCommandExecutor>()
                .AddSingleton<IPrintAndShowHtmlCommandExecutor, PrintAndShowHtmlCommandExecutor>()
                .AddSingleton<IPrintHtmlCommandExecutor, PrintHtmlCommandExecutor>()
                .AddSingleton<IPushCommandExecutor, PushCommandExecutor>()
                .AddSingleton<IRemoveCommandExecutor, RemoveCommandExecutor>()
                .AddSingleton<IScoreCommandExecutor, ScoreCommandExecutor>()
                .AddSingleton<IShowConflictsCommandExecutor, ShowConflictsCommandExecutor>()
                .AddSingleton<IShowHelpCommandExecutor, ShowHelpCommandExecutor>()
                .AddSingleton<IShowHtmlCommandExecutor, ShowHtmlCommandExecutor>()
                .AddSingleton<IShowSettingsCommandExecutor, ShowSettingsCommandExecutor>()
                .AddSingleton<IShowWebpageCommandExecutor, ShowWebpageCommandExecutor>()
                .AddSingleton<IStatusCommandExecutor, StatusCommandExecutor>()
                .AddSingleton<ISyncCommandExecutor, SyncCommandExecutor>()
                .AddSingleton<IUnarchiveCommandExector, UnarchiveCommandExecutor>()
                .AddSingleton<IWhichTodoCommandExecutor, WhichTodoCommandExecutor>();
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072")]
        private IServiceCollection RegisterTypes<T>(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (type == typeof(WhichTodoCommandFactory)) continue;

                serviceCollection.AddSingleton(type);
                serviceCollection.AddSingleton(typeof(T), type);
            }

            return serviceCollection;
        }

        private IServiceCollection AddTypeSets()
            => serviceCollection
                .AddSingleton<ICommandFactorySet, CommandFactorySet>()
                .AddSingleton<ICommandExecutorSet, CommandExecutorSet>();

        private IServiceCollection AddTodoService()
            => serviceCollection
                .AddSingleton<ITodoService, TodoService>();
    }
}
