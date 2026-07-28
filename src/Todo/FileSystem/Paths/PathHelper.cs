using System;
using System.IO;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem.Paths;

/// <summary>
/// A helper class which helps with path manipulation.
/// </summary>
public class PathHelper(IPathEnvironmentVariableRetriever pathEnvironmentVariableRetriever)
    : IPathHelper
{
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

    public string GetWorkingFolder() => Environment.CurrentDirectory;

    public string ResolveIfNotRooted(string path)
    {
        if (Path.IsPathRooted(path))
            return path;

        var paths = pathEnvironmentVariableRetriever.Paths;

        foreach (var candidateFolder in paths)
        {
            var candidatePath = Path.Combine(candidateFolder, path);
            var formattedPath = Path.GetFullPath(candidatePath);

            if (File.Exists(formattedPath))
                return formattedPath;
        }

        throw new Exception($"{path} not found");
    }
}