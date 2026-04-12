using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Todo.AppLaunching;
using Todo.AssemblyOperations;
using Todo.CommandFactories;
using Todo.Contracts.Data.Commands;
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
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;
using Todo.Dates;
using Todo.Dates.Naming;
using Todo.Execution;
using Todo.FileSystem;
using Todo.FileSystem.Paths;
using Todo.GamifyOperations;
using Todo.Git;
using Todo.MarkdownOperations;
using Todo.StateAndConfig;
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
            .AddGitFunctionality()
            .AddGamifyOperations()
            .AddMarkdownFunctionality()
            .AddUiFunctionality()

            /* Command interpretation and execution */
            .AutoRegisterTypes<ICommandFactory<CommandBase>>()
            .AutoRegisterTypes<ICommandExecutor>()
            .AddTypeSets()

            /* Main service */
            .AddTodoService();
    
    public static IServiceProvider GetServiceProvider()
        =>  GetServiceCollection()

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
                    .AddSingleton<IFilenameDateParser, FilenameDateParser>();

            private IServiceCollection AddTemplateFunctionality()
                => serviceCollection
                    .AddSingleton<IDayListMarkdownSubstitutionsMaker, DayListMarkdownSubstitutionsMaker>()
                    .AddSingleton<ITopicListMarkdownSubstitutionsMaker, TopicListMarkdownSubstitutionsMaker>()
                    .AddSingleton<IHtmlSubstitutionsMaker, HtmlSubstitutionsMaker>()
                    .AddSingleton<IDayListMarkdownTemplateProvider, DayListMarkdownTemplateProvider>()
                    .AddSingleton<ITopicListMarkdownTemplateProvider, TopicListMarkdownTemplateProvider>()
                    .AddSingleton<IHtmlTemplateProvider, HtmlTemplateProvider>();

            private IServiceCollection AddFileSystemFunctionality()
                => serviceCollection
                    .AddSingleton<IPathHelper, PathHelper>()
                    .AddSingleton<IOutputFolderPathProvider, OutputFolderPathProvider>()
                    .AddSingleton<IPathEnvironmentVariableRetriever, PathEnvironmentVariableRetriever>()
                    .AddSingleton<IDateListPathResolver, DateListPathResolver>()
                    .AddSingleton<ITopicListPathResolver, TopicListPathResolver>()
                    .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                    .AddSingleton<IFileDeleter, FileDeleter>()
                    .AddSingleton<IFolderCreator, FolderCreator>()
                    .AddSingleton<IFileListCreator, FileListCreator>();

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
            private IServiceCollection AutoRegisterTypes<T>()
            {
                var typesToRegister = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsClass: true, IsAbstract: false })
                    .Where(x => x.IsAssignableTo(typeof(T)));

                foreach (var typeToRegister in typesToRegister)
                {
                    serviceCollection.AddSingleton(typeToRegister);
                    serviceCollection.AddSingleton(typeof(T), typeToRegister);
                }

                var interfacesToMap = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x is { IsInterface: true, IsGenericType: false } && x != typeof(T))
                    .Where(x => x.IsAssignableTo(typeof(T)));

                foreach (var interfaceToRegister in interfacesToMap)
                {
                    var typesAssignableToInterface = Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(x => x is { IsClass: true, IsAbstract: false })
                        .Where(x => x.IsAssignableTo(interfaceToRegister));

                    foreach (var typeAssignableToInterface in typesAssignableToInterface)
                    {
                        serviceCollection.AddSingleton(interfaceToRegister,
                            x => x.GetRequiredService(typeAssignableToInterface));
                    }
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
