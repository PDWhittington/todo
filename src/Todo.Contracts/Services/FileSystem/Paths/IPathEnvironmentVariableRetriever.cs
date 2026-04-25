namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IPathEnvironmentVariableRetriever
{
    public IEnumerable<string> Paths { get; }
}
