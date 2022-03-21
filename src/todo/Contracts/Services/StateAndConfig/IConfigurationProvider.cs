using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IConfigurationProvider
{
    public Configuration Config { get; }

    public void Reset();
}
