using System;
using System.IO;
using System.Text.Json;
using Todo.Contracts.Data;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private ConfigurationInfo? _configuration;

    public ConfigurationProvider(ISettingsPathProvider settingsPathProvider)
    {
        _settingsPathProvider = settingsPathProvider;
    }

    public ConfigurationInfo Config => _configuration ?? PopulateAndReturnConfiguration();

    private ConfigurationInfo PopulateAndReturnConfiguration()
    {
        using var fileStream = new FileStream(_settingsPathProvider.SettingsPath.Path, FileMode.Open, FileAccess.Read);

        var serializationOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

        var configuration = JsonSerializer.Deserialize<ConfigurationInfo>(fileStream, serializationOptions);

        _configuration = configuration ??
                         throw new Exception($"Configuration could not be loaded from {_settingsPathProvider.SettingsPath.Path}");

        return _configuration;
    }
}
