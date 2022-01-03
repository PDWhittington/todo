using Todo.Contracts.Data;

namespace Todo.Contracts.Services.StateAndConfig
{
    public interface IConfigurationProvider
    {
        public ConfigurationInfo Config { get; }
    }
}