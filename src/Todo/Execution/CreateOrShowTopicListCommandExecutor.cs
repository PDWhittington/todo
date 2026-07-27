using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
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
    IFolderCreator folderCreator,
    ILogger<CreateOrShowTopicListCommandExecutor> logger)
    : CreateOrShowCommandExecutorBase<CreateOrShowTopicListCommand, TopicListMarkdownSubstitutions>(
            configurationProvider, gitInterface, fileOpener, outputWriter, folderCreator, logger),
        ICreateOrShowTopicListCommandExecutor
{
    protected override FilePathInfo GetFilePathInfo(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => topicListPathResolver.ResolvePathFor(createOrShowTopicListCommand.Topic, FileTypeEnum.MarkdownTopicList, true);

    protected override TodoFile GetTemplate() => topicListMarkdownTemplateProvider.GetTemplate();

    protected override TopicListMarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowTopicListCommand createOrShowTopicListCommand)
        => TopicListMarkdownSubstitutions.Of(createOrShowTopicListCommand.Topic);

    protected override void MakeSubstitutions(TopicListMarkdownSubstitutions markdownSubstitutions, 
        UnmanagedByteArray fileContents, Stream stream)
        => topicListMarkdownSubstitutionsMaker.WriteSubstitutionsToStream(fileContents, 
            markdownSubstitutions, stream); 
}
