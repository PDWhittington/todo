using System;
using System.IO;
using System.Text.Json;
using Todo.Contracts.Data.Caching;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConfigurationProvider : IConfigurationProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IConstantsProvider _constantsProvider;
    private readonly ResettableLazy<ConfigurationInfo> _configuration;

    public ConfigurationInfo ConfigInfo => _configuration.Value;

    public void Reset() => _configuration.Reset();
    
    public ConfigurationProvider(ISettingsPathProvider settingsPathProvider,
        IConstantsProvider constantsProvider)
    {
        _settingsPathProvider = settingsPathProvider;
        _constantsProvider = constantsProvider;
        _configuration = new ResettableLazy<ConfigurationInfo>(GetConfiguration);
    }

    private ConfigurationInfo GetConfiguration()
    {   
        var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        
        var configuration = JsonSerializer.Deserialize<Configuration>(fileStream, 
                AppJsonContext.Default.Configuration)
                ?? throw new Exception($"Configuration could not be loaded from {path}");

        return ConfigurationInfo.Of(path, configuration);
    }
}