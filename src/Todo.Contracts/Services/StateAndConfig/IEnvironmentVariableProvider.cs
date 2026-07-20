namespace Todo.Contracts.Services.StateAndConfig;

public interface IEnvironmentVariableProvider
{
    bool TryGetEnvironmentVariable(string? key, out string? value);
}