namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IPathHelper
{
    string GetRootedToAssemblyFolder(string path);

    string GetRootedToWorkingFolder(string path);

    string GetAssemblyLocation();

    string GetWorkingFolder();

    string ResolveIfNotRooted(string path);
}
