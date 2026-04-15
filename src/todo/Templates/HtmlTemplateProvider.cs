using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class HtmlTemplateProvider(
    IPathHelper pathHelper,
    IManifestStreamProvider manifestStreamProvider,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter)
    : TemplateProviderBase(pathHelper, manifestStreamProvider, markdownLineInterpreter), IHtmlTemplateProvider
{
    protected override string GetTemplateFileName()
        => constantsProvider.DefaultHtmlTemplate.FileName;

    protected override string GetManifestStreamName()
        => constantsProvider.DefaultHtmlTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.HtmlTemplate;

    protected override string GetTemplateDescription() => "html";
}
