using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.MarkdownOperations;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class DayListMarkdownTemplateProvider(
    IPathHelper pathHelper,
    IManifestStreamProvider manifestStreamProvider,
    IConstantsProvider constantsProvider,
    IMarkdownLineInterpreter markdownLineInterpreter)
    : TemplateProviderBase(pathHelper, manifestStreamProvider, markdownLineInterpreter),
        IDayListMarkdownTemplateProvider
{
    protected override string GetTemplateFileName()
        => constantsProvider.DayListMarkdownTemplate.FileName;

    protected override string GetManifestStreamName()
        => constantsProvider.DayListMarkdownTemplate.FullName;

    protected override FileTypeEnum GetFileType()
        => FileTypeEnum.MarkdownTemplate;

    protected override string GetTemplateDescription() => "daylist markdown";
}
