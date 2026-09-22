using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class ListHtmlTemplateProvider(
    IAssemblyInformationProvider assemblyInformationProvider,
    IPathHelper pathHelper,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter,
    IUnmanagedByteArrayManager unmanagedByteArrayManager,
    IFileSystemFactory fileSystemFactory)
    : TemplateProviderBase(assemblyInformationProvider, pathHelper, markdownLineInterpreter, unmanagedByteArrayManager, fileSystemFactory), 
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