using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Todo.CommandFactories;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
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

internal static class Initialise
{
    public static ServiceProvider GetServiceProvider()
            =>  new ServiceCollection()
                .AddLogging()

                /* Base functionality */
                .AddStateAndConfig()
                .AddHelpers()
                .AddTemplateFunctionality()
                .AddDateNaming()
                .AddFileSystemFunctionality()
                .AddHelpTextWritingFunctionality()
                .AddGitFunctionality()

                /* Command interpretation and execution */
                .AutoRegisterTypes<ICommandFactory<CommandBase>>()
                .AutoRegisterTypes<IExecutor>()
                .AddTypeSets()

                /* Main service */
                .AddTodoService()

                /* Build the service provider */
                .BuildServiceProvider();

        #region Base functionality

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
                .AddSingleton<IPathResolver, PathResolver>()
                .AddSingleton<IContentFilePathResolver, ContentFilePathResolver>()
                .AddSingleton<IFileReader, FileReader>()
                .AddSingleton<IMarkdownFileReader, MarkdownFileReader>()
                .AddSingleton<IFileOpener, FileOpener>()
                .AddSingleton<IFileDeleter, FileDeleter>();

        private static IServiceCollection AddHelpTextWritingFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IConsoleTextFormatter, ConsoleTextFormatter>();

        private static IServiceCollection AddGitFunctionality(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IGitDependencyValidator, GitDependencyValidator>()
                .AddSingleton<IGitInterface, GitInterface>();

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
