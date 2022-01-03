using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

/// <summary>
/// A client of this class can retrieve a string which is a template of the markdown to use.
/// </summary>
public class MarkdownTemplateProvider : TemplateProviderBase, IMarkdownTemplateProvider
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;

    /// <summary>
    /// Constructor. This takes a ConfigurationProvider
    /// </summary>
    /// <param name="configurationProvider">Typically registered in Windsor</param>
    /// <param name="pathHelper"></param>
    public MarkdownTemplateProvider(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    protected override FilePathInfo GetTemplatePath()
    {
        var path = _pathHelper.GetRooted(_configurationProvider.Config.MarkdownTemplatePath);
        return FilePathInfo.Of(path, FileTypeEnum.MarkdownTemplate, FolderEnum.SpecifiedInSettings);
    }
}
