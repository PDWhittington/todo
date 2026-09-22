using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem.Paths;

/// <summary>
/// A helper class which helps with path manipulation.
/// </summary>
public class PathHelper(
    IPathEnvironmentVariableRetriever pathEnvironmentVariableRetriever,
    IFileSystemFactory fileSystemFactory)
    : IPathHelper
{
    private readonly IFileSystem _fileSystem = fileSystemFactory.Create();

    /// <summary>
    /// Roots the path to the working folder,
    /// unless the path is already rooted.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string GetRootedToWorkingFolder(string path)
    {
        var rootedPath = _fileSystem.IsPathRooted(path)
            ? path
            : _fileSystem.Combine(GetWorkingFolder(), path);
        return _fileSystem.GetFullPath(rootedPath);
    }

    public string GetWorkingFolder() => Environment.CurrentDirectory;

    public string ResolveIfNotRooted(string path)
    {
        if (_fileSystem.IsPathRooted(path))
            return path;

        var paths = pathEnvironmentVariableRetriever.Paths;

        foreach (var candidateFolder in paths)
        {
            var candidatePath = _fileSystem.Combine(candidateFolder, path);
            var formattedPath = _fileSystem.GetFullPath(candidatePath);

            if (File.Exists(formattedPath))
                return formattedPath;
        }

        throw new Exception($"{path} not found");
    }
}