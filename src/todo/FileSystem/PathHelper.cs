using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class PathHelper : IPathHelper
{
    public string GetRootedToAssemblyFolder(string path)
        => Path.IsPathRooted(path) ? path : Path.Combine(GetAssemblyFolder(), path);

    public string GetRootedToWorkingFolder(string path)
        => Path.IsPathRooted(path) ? path : Path.Combine(GetWorkingFolder(), path);

    public string GetAssemblyFolder()
        => Path.GetDirectoryName(GetAssemblyLocation()) ??
           throw new Exception("Cannot get containing folder of executing process");

    public string GetAssemblyLocation() => Assembly.GetEntryAssembly()?.Location!;

    public string GetWorkingFolder() => Environment.CurrentDirectory;
}
