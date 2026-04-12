using System;
using System.Drawing;
using System.IO;
using Todo.Contracts.Data.Caching;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Utf8Json;
using Utf8Json.Resolvers;

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
        var resolver = CompositeResolver.Create(
            [
                new ColorFormatter()
            ],
            [
                StandardResolver.Default
            ]);
        
        CompositeResolver.RegisterAndSetAsDefault(
            new IJsonFormatter[] { new ColorFormatter() },
            new IJsonFormatterResolver[] { StandardResolver.Default }
            );

        var test = CompositeResolver.Instance.GetFormatter<Color>();
        
        var path =  _settingsPathProvider.GetSettingsPathInHierarchy().Path ??
                    throw new FileNotFoundException($"{_constantsProvider.SettingsFileName} not found.",
                        _constantsProvider.SettingsFileName);

        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        
        var configuration = JsonSerializer.Deserialize<Configuration>(fileStream, CustomResolver.Instance)
                            ?? throw new Exception($"Configuration could not be loaded from {path}");

        return ConfigurationInfo.Of(path, configuration);
    }
}

public class CustomResolver : IJsonFormatterResolver
{
    public static readonly CustomResolver Instance = new();

    private readonly IJsonFormatterResolver _inner = StandardResolver.Default;

    public IJsonFormatter<T> GetFormatter<T>()
    {
        if (typeof(T) == typeof(Color))
            return (IJsonFormatter<T>)new ColorFormatter();

        return _inner.GetFormatter<T>();
    }
}