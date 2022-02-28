using System;
using System.IO;
using System.Text.Json;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IConstantsProvider _constantsProvider;
    private readonly Lazy<ConfigurationInfo> _configuration;

    public ConfigurationProvider(ISettingsPathProvider settingsPathProvider,
        IConstantsProvider constantsProvider)
    {
        _settingsPathProvider = settingsPathProvider;
        _constantsProvider = constantsProvider;
        _configuration = new Lazy<ConfigurationInfo>(PopulateAndReturnConfiguration);
    }

    public ConfigurationInfo Config => _configuration.Value;

    private ConfigurationInfo PopulateAndReturnConfiguration()
    {
        var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var serializationOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

        var configuration = JsonSerializer.Deserialize<ConfigurationInfo>(fileStream, serializationOptions);

        return configuration ?? throw new Exception(
            $"Configuration could not be loaded from {path}");
    }
}
