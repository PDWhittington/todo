namespace Todo.Contracts.Services.Helpers;

public interface IPathHelper
{
    string GetRootedToAssemblyFolder(string path);

    string GetRootedToWorkingFolder(string path);

    string GetAssemblyFolder();

    string GetAssemblyLocation();

    string GetWorkingFolder();
}
