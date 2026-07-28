using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class TopicListMarkdownTemplateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IPathHelper pathHelper,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter,
    IUnmanagedByteArrayManager unmanagedByteArrayManager)
    : TemplateProviderBase(assemblyInformationProvider, pathHelper, markdownLineInterpreter, unmanagedByteArrayManager),
        ITopicListMarkdownTemplateProvider
{
    protected override string GetTemplateFileName()
        => constantsProvider.TopicListMarkdownTemplate.FileName;

    protected override string GetManifestStreamName()
        => constantsProvider.TopicListMarkdownTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.MarkdownTemplate;

    protected override string GetTemplateDescription() => "topic list description";
}