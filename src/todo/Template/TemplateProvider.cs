using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Template
{
    /// <summary>
    /// A client of this class can retrieve a string which is a template of the markdown to use.
    /// </summary>
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPathHelper _pathHelper;

        /// <summary>
        /// Constructor. This takes a ConfigurationProvider
        /// </summary>
        /// <param name="configurationProvider">Typically registered in Windsor</param>
        /// <param name="pathHelper"></param>
        public TemplateProvider(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
        {
            _configurationProvider = configurationProvider;
            _pathHelper = pathHelper;
        }
        
        /// <summary>
        /// Returns a string representing the Markdown template
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTemplate()
        {
            var path = _configurationProvider.GetConfiguration().TemplatePath;
            var rootedPath = _pathHelper.GetRooted(path);
            
            if (!File.Exists(rootedPath)) throw new Exception($"{rootedPath} not found");

            return File.ReadAllText(rootedPath);
        }
    }
}