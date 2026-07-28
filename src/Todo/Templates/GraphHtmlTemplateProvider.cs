using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class GraphHtmlTemplateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IPathHelper pathHelper,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter,
    IUnmanagedByteArrayManager unmanagedByteArrayManager)
    : TemplateProviderBase(assemblyInformationProvider, pathHelper, markdownLineInterpreter, unmanagedByteArrayManager), 
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