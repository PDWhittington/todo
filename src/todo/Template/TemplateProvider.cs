using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services;

namespace Todo.Template
{
    /// <summary>
    /// A client of this class can retrieve a string which is a template of the markdown to use.
    /// </summary>
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        /// <summary>
        /// Constructor. This takes a ConfigurationProvider
        /// </summary>
        /// <param name="configurationProvider">Typically registered in Windsor</param>
        public TemplateProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
        
        /// <summary>
        /// Returns a string representing the Markdown template
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTemplate()
        {
            var path = _configurationProvider.GetConfiguration().TemplatePath;

            if (!File.Exists(path)) throw new Exception($"{path} not found");

            return File.ReadAllText(path);
        }
    }
}