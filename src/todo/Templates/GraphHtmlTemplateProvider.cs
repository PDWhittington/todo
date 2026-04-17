using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class GraphHtmlTemplateProvider(
    IPathHelper pathHelper,
    IManifestStreamProvider manifestStreamProvider,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter)
    : TemplateProviderBase(pathHelper, manifestStreamProvider, markdownLineInterpreter), 
        IGraphHtmlTemplateProvider
{
    protected override string GetTemplateFileName()
        => constantsProvider.GraphHtmlTemplate.FileName;

    protected override string GetManifestStreamName()
        => constantsProvider.GraphHtmlTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.HtmlTemplate;

    protected override string GetTemplateDescription() => "html";
}
