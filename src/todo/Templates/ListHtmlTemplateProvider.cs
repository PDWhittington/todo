using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class ListHtmlTemplateProvider(
    IPathHelper pathHelper,
    IManifestStreamProvider manifestStreamProvider,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter)
    : TemplateProviderBase(pathHelper, manifestStreamProvider, markdownLineInterpreter), 
        IListHtmlTemplateProvider
{
    protected override string GetTemplateFileName()
        => constantsProvider.ListHtmlTemplate.FileName;

    protected override string GetManifestStreamName()
        => constantsProvider.ListHtmlTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.HtmlTemplate;

    protected override string GetTemplateDescription() => "html";
}
