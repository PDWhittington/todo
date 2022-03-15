using System;
using System.IO;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Utf8Json;

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
        _configuration = new Lazy<ConfigurationInfo>(GetConfiguration);
    }

    public ConfigurationInfo Config => _configuration.Value;

    private ConfigurationInfo GetConfiguration()
    {
        var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var configuration = JsonSerializer.Deserialize<ConfigurationInfo>(fileStream);

        return configuration ?? throw new Exception(
            $"Configuration could not be loaded from {path}");
    }
}
