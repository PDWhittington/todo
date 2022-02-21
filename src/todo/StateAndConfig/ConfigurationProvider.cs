using System;
using System.IO;
using System.Text.Json;
using Todo.Contracts.Data;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IConstantsProvider _constantsProvider;
    private ConfigurationInfo? _configuration;

    public ConfigurationProvider(ISettingsPathProvider settingsPathProvider, IConstantsProvider constantsProvider)
    {
        _settingsPathProvider = settingsPathProvider;
        _constantsProvider = constantsProvider;
    }

    public ConfigurationInfo Config => _configuration ?? PopulateAndReturnConfiguration();

    private ConfigurationInfo PopulateAndReturnConfiguration()
    {
        var path =  _settingsPathProvider.SettingsPathInHierarchy?.Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var serializationOptions = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };

        var configuration = JsonSerializer.Deserialize<ConfigurationInfo>(fileStream, serializationOptions);

        _configuration = configuration ?? throw new Exception(
            $"Configuration could not be loaded from {path}");

        return _configuration;
    }
}
