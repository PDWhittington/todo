using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IConfigurationProvider
{
    public ConfigurationInfo Config { get; }
}
