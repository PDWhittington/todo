using System;
using System.Runtime.InteropServices;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class PathEnvironmentVariableRetriever : IPathEnvironmentVariableRetriever
{
    private string[]? _paths;

    public string[] Paths => _paths ?? GetPaths();

    private string[] GetPaths()
    {
        var pathVariable = Environment.GetEnvironmentVariable("PATH");

        if (pathVariable == null) throw new Exception("PATH environment variable not found");

        //Paths are separated by ; on Windows, and by : on Mac and Linux

        _paths = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? pathVariable.Split(';')
            : pathVariable.Split(':');

        return _paths;
    }
}
