using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class EnvironmentPathResolver : IEnvironmentPathResolver
{
    private readonly IPathEnvironmentVariableRetriever _pathEnvironmentVariableRetriever;

    public EnvironmentPathResolver(IPathEnvironmentVariableRetriever pathEnvironmentVariableRetriever)
    {
        _pathEnvironmentVariableRetriever = pathEnvironmentVariableRetriever;
    }

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
