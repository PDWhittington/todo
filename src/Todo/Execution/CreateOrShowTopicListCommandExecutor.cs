using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowTopicListCommandExecutor(
    ITopicListMarkdownTemplateProvider topicListMarkdownTemplateProvider,
    ITopicListPathResolver topicListPathResolver,
    ITopicListMarkdownSubstitutionsMaker topicListMarkdownSubstitutionsMaker,
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    ITextFileLauncher fileOpener,
    IOutputWriter outputWriter,
    IFolderCreator folderCreator)
    : CreateOrShowCommandExecutorBase<CreateOrShowTopicListCommand, TopicListMarkdownSubstitutions>(
            configurationProvider, gitInterface, fileOpener, outputWriter, folderCreator),
        ICreateOrShowTopicListCommandExecutor
{
    protected override FilePathInfo GetFilePathInfo(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => topicListPathResolver.ResolvePathFor(createOrShowTopicListCommand.Topic, FileTypeEnum.MarkdownTopicList, true);

    protected override TodoFile GetTemplate() => topicListMarkdownTemplateProvider.GetTemplate();

    protected override TopicListMarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => TopicListMarkdownSubstitutions.Of(createOrShowTopicListCommand.Topic);

    protected override string MakeSubstitutions(TopicListMarkdownSubstitutions markdownSubstitutions, string fileContents)
        => topicListMarkdownSubstitutionsMaker.MakeSubstitutions(markdownSubstitutions, fileContents);
}
