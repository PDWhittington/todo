namespace Todo.Contracts.Services.FileSystem;

public interface IPathEnvironmentVariableRetriever
{
    public string[] Paths { get; }
}
