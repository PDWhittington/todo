using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

public class HtmlTemplateProvider : TemplateProviderBase, IHtmlTemplateProvider
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;

    /// <summary>
    /// Constructor. This takes a ConfigurationProvider
    /// </summary>
    /// <param name="configurationProvider">Typically registered in Windsor</param>
    /// <param name="pathHelper"></param>
    public HtmlTemplateProvider(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    protected override string GetTemplatePath() => _pathHelper.GetRooted(_configurationProvider.Config.HtmlTemplatePath);
}
