namespace Todo.Contracts.Services
{
    public interface IConfigurationProvider
    {
        Data.ConfigurationInfo Configuration { get; }
    }
}