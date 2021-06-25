using Todo.Contracts.Services;

namespace Todo.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private Contracts.Data.ConfigurationInfo _configuration;

        public Contracts.Data.ConfigurationInfo Configuration => _configuration ??= Contracts.Data.ConfigurationInfo.Of("template.md");
    }
}