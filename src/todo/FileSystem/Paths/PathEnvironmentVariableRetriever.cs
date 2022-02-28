using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem.Paths;

/// <summary>
/// A class which retrieves the PATH environment variable, and splits the string
/// into an array of paths. The correct delimiter is used for the operating system
/// in use.
/// </summary>
public class PathEnvironmentVariableRetriever : IPathEnvironmentVariableRetriever
{
    private string[]? _paths;

    /// <summary>
    /// Sticky environment paths property, which is populated once per process.
    /// </summary>
    public IEnumerable<string> Paths => _paths ?? RetrieveAndPopulatePaths();

    /// <summary>
    /// Retrieves and populates the paths from the environment variable.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string[] RetrieveAndPopulatePaths()
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
