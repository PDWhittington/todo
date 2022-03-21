namespace Todo.Contracts.Data.Config;

public class ConfigurationInfo
{
    public string Path { get; }

    public Configuration Configuration { get; }

    private ConfigurationInfo(string path, Configuration configuration)
    {
        Path = path;
        Configuration = configuration;
    }

    public static ConfigurationInfo Of(string path, Configuration configuration)
        => new(path, configuration);
}
