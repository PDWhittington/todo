namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IPathHelper
{
    string GetRootedToWorkingFolder(string path);

    string GetWorkingFolder();

    string ResolveIfNotRooted(string path);
}
