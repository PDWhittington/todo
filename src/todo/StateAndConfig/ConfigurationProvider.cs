using System.IO;
using System.Text.Json;
using Todo.Contracts.Data;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly ISettingsPathProvider _settingsPathProvider;
        private ConfigurationInfo _configuration = null;

        public ConfigurationProvider(ISettingsPathProvider settingsPathProvider)
        {
            _settingsPathProvider = settingsPathProvider;
        }
        
        public ConfigurationInfo Config => _configuration ?? PopulateAndReturnConfiguration();
        
        private ConfigurationInfo PopulateAndReturnConfiguration()
        {
            var settingsPath = _settingsPathProvider.GetSettingsPath();

            using var fileStream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read);
            var configuration = JsonSerializer.Deserialize<ConfigurationInfo>(fileStream);

            _configuration = configuration;
            return _configuration;
        }
    }
}