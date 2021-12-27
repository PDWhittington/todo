using Todo.Contracts.Data;

namespace Todo.Contracts.Services
{
    public interface IConfigurationProvider
    {
        public ConfigurationInfo GetConfiguration();
    }
}