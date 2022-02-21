namespace Todo.Contracts.Services.FileSystem;

public interface IEnvironmentPathResolver
{
    string ResolveIfNotRooted(string path);
}
