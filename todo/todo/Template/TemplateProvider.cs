using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services;

namespace Todo.Template
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        public TemplateProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }
        
        public string GetTemplate()
        {
            var templatePath = _configurationProvider.Configuration.TemplatePath;

            var fullPath = Path.IsPathRooted(templatePath) ? templatePath : RootToAssemblyFolder(templatePath);

            if (!File.Exists(fullPath)) throw new Exception($"{fullPath} not found");

            return File.ReadAllText(fullPath);
        }

        private string RootToAssemblyFolder(string fileName)
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? throw new Exception("Cannot find folder of the executing process");
            var assemblyFolder = Path.GetDirectoryName(assemblyLocation) ?? throw new Exception("Cannot get containing folder of executing process");;

            return Path.Combine(assemblyFolder, fileName);
        }
    }
}