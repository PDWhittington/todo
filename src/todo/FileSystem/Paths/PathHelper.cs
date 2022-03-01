using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem.Paths;

/// <summary>
/// A helper class which helps with path manipulation.
/// </summary>
public class PathHelper : IPathHelper
{
    private readonly IPathEnvironmentVariableRetriever _pathEnvironmentVariableRetriever;

    public PathHelper(IPathEnvironmentVariableRetriever pathEnvironmentVariableRetriever)
    {
        _pathEnvironmentVariableRetriever = pathEnvironmentVariableRetriever;
    }

    /// <summary>
    /// Roots the path to the folder containing the executing assembly,
    /// unless the path is already rooted.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetRootedToAssemblyFolder(string path)
    {
        var rootedPath = Path.IsPathRooted(path) ? path : Path.Combine(GetAssemblyFolder(), path);
        return Path.GetFullPath(rootedPath); //Use this to format the paths with native / or \
    }


    /// <summary>
    /// Roots the path to the working folder,
    /// unless the path is already rooted.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetRootedToWorkingFolder(string path)
    {
        var rootedPath = Path.IsPathRooted(path) ? path : Path.Combine(GetWorkingFolder(), path);
        return Path.GetFullPath(rootedPath); //Use this to format the paths with native / or \
    }

    /// <summary>
    /// Returns the folder containing the executing assembly
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetAssemblyFolder()
        => Path.GetDirectoryName(GetAssemblyLocation()) ??
           throw new Exception("Cannot get containing folder of executing process");

    /// <summary>
    /// Returns the location of the executing assembly
    /// </summary>
    /// <returns></returns>
    public string GetAssemblyLocation() => Assembly.GetEntryAssembly()?.Location!;

    public string GetWorkingFolder() => Environment.CurrentDirectory;

    public string ResolveIfNotRooted(string path)
    {
        if (Path.IsPathRooted(path)) return path;

        var paths = _pathEnvironmentVariableRetriever.Paths;

        foreach (var candidateFolder in paths)
        {
            var candidatePath = Path.Combine(candidateFolder, path);
            var formattedPath = Path.GetFullPath(candidatePath);

            if (File.Exists(formattedPath)) return candidatePath;
        }

        throw new Exception($"{path} not found");
    }
}
