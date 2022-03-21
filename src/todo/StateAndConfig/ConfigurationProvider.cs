using System;
using System.IO;
using Todo.Contracts.Data.Caching;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Utf8Json;

namespace Todo.StateAndConfig;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IConstantsProvider _constantsProvider;
    private readonly ResettableLazy<ConfigurationInfo> _configuration;

    public ConfigurationProvider(ISettingsPathProvider settingsPathProvider,
        IConstantsProvider constantsProvider)
    {
        _settingsPathProvider = settingsPathProvider;
        _constantsProvider = constantsProvider;
        _configuration = new ResettableLazy<ConfigurationInfo>(GetConfiguration);
    }

    public ConfigurationInfo ConfigInfo => _configuration.Value;

    public void Reset() => _configuration.Reset();

    private ConfigurationInfo GetConfiguration()
    {
        var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var configuration = JsonSerializer.Deserialize<Configuration>(fileStream)
                            ?? throw new Exception($"Configuration could not be loaded from {path}");

        return ConfigurationInfo.Of(path, configuration);
    }
}
