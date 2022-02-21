namespace Todo.Contracts.Services.FileSystem;

public interface IPathRootingProvider
{
    string GetRootedOutputFolder();

    string GetRootedArchiveFolder();
}
