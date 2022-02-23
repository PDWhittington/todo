using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Templates;

/// <summary>
/// A client of this class can retrieve a string which is a template of the markdown to use.
/// </summary>
public class MarkdownTemplateProvider : TemplateProviderBase<MarkdownTemplateEnum>,
    IMarkdownTemplateProvider
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

    protected override FilePathInfo GetTemplatePath(MarkdownTemplateEnum key)
    {
        var pathToUse = key switch
        {
            MarkdownTemplateEnum.DayListTemplate => _configurationProvider.Config.DayListMarkdownTemplatePath,
            MarkdownTemplateEnum.TopicListTemplate => _configurationProvider.Config.TopicListMarkdownTemplatePath,
            _ => throw new ArgumentException(null, nameof(key))
        };

        var rootedPath = _pathHelper.GetRootedToAssemblyFolder(pathToUse);
        return FilePathInfo.Of(rootedPath, FileTypeEnum.MarkdownTemplate, FolderEnum.SpecifiedInSettings);
    }
}
