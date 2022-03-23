using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
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
public class CreateOrShowTopicListCommandExecutor
    : CreateOrShowCommandExecutorBase<CreateOrShowTopicListCommand, TopicListMarkdownSubstitutions>,
        ICreateOrShowTopicListCommandExecutor
{
    private readonly ITopicListMarkdownTemplateProvider _topicListMarkdownTemplateProvider;
    private readonly ITopicListPathResolver _topicListPathResolver;
    private readonly ITopicListMarkdownSubstitutionsMaker _topicListMarkdownSubstitutionsMaker;

    public CreateOrShowTopicListCommandExecutor(ITopicListMarkdownTemplateProvider topicListMarkdownTemplateProvider,
        ITopicListPathResolver topicListPathResolver,
        ITopicListMarkdownSubstitutionsMaker topicListMarkdownSubstitutionsMaker,
        IConfigurationProvider configurationProvider, IGitInterface gitInterface,
        ITextFileLauncher fileOpener, IOutputWriter outputWriter, IFolderCreator folderCreator)
        : base(configurationProvider, gitInterface, fileOpener, outputWriter, folderCreator)
    {
        _topicListMarkdownTemplateProvider = topicListMarkdownTemplateProvider;
        _topicListPathResolver = topicListPathResolver;
        _topicListMarkdownSubstitutionsMaker = topicListMarkdownSubstitutionsMaker;
    }

    protected override FilePathInfo GetFilePathInfo(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => _topicListPathResolver.ResolvePathFor(createOrShowTopicListCommand.Topic, FileTypeEnum.MarkdownTopicList, true);

    protected override TodoFile GetTemplate() => _topicListMarkdownTemplateProvider.GetTemplate();

    protected override TopicListMarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => TopicListMarkdownSubstitutions.Of(createOrShowTopicListCommand.Topic);

    protected override string MakeSubstitutions(TopicListMarkdownSubstitutions markdownSubstitutions, string fileContents)
        => _topicListMarkdownSubstitutionsMaker.MakeSubstitutions(markdownSubstitutions, fileContents);
}
