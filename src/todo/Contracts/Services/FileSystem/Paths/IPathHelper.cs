namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IPathHelper
{
    string GetRootedToAssemblyFolder(string path);

    string GetRootedToWorkingFolder(string path);

    // ReSharper disable once UnusedMemberInSuper.Global
    string GetAssemblyFolder();

    string GetAssemblyLocation();

    string GetWorkingFolder();

    string ResolveIfNotRooted(string path);
}
