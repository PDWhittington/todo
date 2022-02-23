using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services.FileSystem;

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
        => Path.IsPathRooted(path) ? path : Path.Combine(GetAssemblyFolder(), path);

    /// <summary>
    /// Roots the path to the working folder,
    /// unless the path is already rooted.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetRootedToWorkingFolder(string path)
        => Path.IsPathRooted(path) ? path : Path.Combine(GetWorkingFolder(), path);

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

            if (File.Exists(candidatePath)) return candidatePath;
        }

        throw new Exception($"{path} not found");
    }
}
