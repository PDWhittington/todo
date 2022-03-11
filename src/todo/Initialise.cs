using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Todo.AppLaunching;
using Todo.AssemblyOperations;
using Todo.CommandFactories;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.Dates.Parsing;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;
using Todo.Dates.Naming;
using Todo.Dates.Parsing;
using Todo.Execution;
using Todo.FileSystem;
using Todo.FileSystem.Paths;
using Todo.Git;
using Todo.StateAndConfig;
using Todo.Templates;
using Todo.UI;

namespace Todo;

internal static class Initialise
{
    public static ServiceProvider GetServiceProvider()
            =>  new ServiceCollection()
                .AddLogging()

                /* Base functionality */
                .AddAppLaunchingOperations()
                .AddAssemblyOperations()
                .AddStateAndConfig()
                .AddDateParsing()
                .AddTemplateFunctionality()
                .AddDateNaming()
                .AddFileSystemFunctionality()
                .AddGitFunctionality()
                .AddUiFunctionality()

                /* Command interpretation and execution */
                .AutoRegisterTypes<ICommandFactory<CommandBase>>()
                .AutoRegisterTypes<ICommandExecutor>()
                .AddTypeSets()

                /* Main service */
                .AddTodoService()

                /* Build the service provider */
                .BuildServiceProvider();

        #region Base functionality

        private static IServiceCollection AddAppLaunchingOperations(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IHtmlFileLauncher, HtmlFileLauncher>()
                .AddSingleton<ITextFileLauncher, TextFileLauncher>();
                
        private static IServiceCollection AddAssemblyOperations(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IManifestStreamProvider, ManifestStreamProvider>();

        private static IServiceCollection AddStateAndConfig(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IConstantsProvider, ConstantsProvider>()
                .AddSingleton<ICommandLineProvider, CommandLineProvider>()
                .AddSingleton<IConfigurationProvider, ConfigurationProvider>()
                .AddSingleton<ICommandProvider, CommandProvider>()
                .AddSingleton<ISettingsPathProvider, SettingsPathProvider>()
                .AddSingleton<IAssemblyInformationProvider, AssemblyInformationProvider>();

        private static IServiceCollection AddDateParsing(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IDateParser, DateParser>()
                .AddSingleton<IDateHelper, DateHelper>();

        private static IServiceCollection AddTemplateFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IDayListMarkdownSubstitutionsMaker, DayListMarkdownSubstitutionsMaker>()
                .AddSingleton<ITopicListMarkdownSubstitutionsMaker, TopicListMarkdownSubstitutionsMaker>()
                .AddSingleton<IHtmlSubstitutionsMaker, HtmlSubstitutionsMaker>()
                .AddSingleton<IDayListMarkdownTemplateProvider, DayListMarkdownTemplateProvider>()
                .AddSingleton<ITopicListMarkdownTemplateProvider, TopicListMarkdownTemplateProvider>()
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
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IOutputFolderPathProvider, OutputFolderPathProvider>()
                .AddSingleton<IPathEnvironmentVariableRetriever, PathEnvironmentVariableRetriever>()
                .AddSingleton<IDateListPathResolver, DateListPathResolver>()
                .AddSingleton<ITopicListPathResolver, TopicListPathResolver>()
                .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                .AddSingleton<IFileDeleter, FileDeleter>();

        private static IServiceCollection AddGitFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGitInterface, GitInterface>();

        private static IServiceCollection AddUiFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IConsoleTextFormatter, ConsoleTextFormatter>()
                .AddSingleton<IOutputWriter, OutputWriter>();

        #endregion

        private static IServiceCollection AutoRegisterTypes<T>(this IServiceCollection serviceCollection)
        {
            var typesToRegister = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract)
                .Where(x => x.IsAssignableTo(typeof(T)));

            foreach (var typeToRegister in typesToRegister)
            {
                serviceCollection.AddSingleton(typeToRegister);
                serviceCollection.AddSingleton(typeof(T), typeToRegister);
            }

            var interfacesToMap = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsInterface && !x.IsGenericType && x != typeof(T))
                .Where(x => x.IsAssignableTo(typeof(T)));

            foreach (var interfaceToRegister in interfacesToMap)
            {
                var typesAssignableToInterface = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract)
                    .Where(x => x.IsAssignableTo(interfaceToRegister));

                foreach (var typeAssignableToInterface in typesAssignableToInterface)
                {
                    serviceCollection.AddSingleton(interfaceToRegister,
                        x => x.GetRequiredService(typeAssignableToInterface));
                }
            }

            return serviceCollection;
        }

        private static IServiceCollection AddTypeSets(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ICommandFactorySet, CommandFactorySet>()
                .AddSingleton<ICommandExecutorSet, CommandExecutorSet>();

        private static IServiceCollection AddTodoService(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<ITodoService, TodoService>();
}
