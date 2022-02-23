namespace Todo.Contracts.Services.FileSystem;

public interface IPathHelper
{
    string GetRootedToAssemblyFolder(string path);

    string GetRootedToWorkingFolder(string path);

    string GetAssemblyFolder();

    string GetAssemblyLocation();

    string GetWorkingFolder();

    string ResolveIfNotRooted(string path);
}
