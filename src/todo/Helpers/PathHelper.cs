using System;
using System.IO;
using System.Reflection;
using Todo.Contracts.Services.Helpers;

namespace Todo.Helpers;

public class PathHelper : IPathHelper
{
    public string GetRooted(string path)
        => Path.IsPathRooted(path) ? path : Path.Combine(GetAssemblyFolder(), path);

    public string GetAssemblyFolder()
        => System.IO.Path.GetDirectoryName(GetAssemblyLocation()) ??
           throw new Exception("Cannot get containing folder of executing process");

    public string GetAssemblyLocation() => Assembly.GetEntryAssembly()?.Location!;
}
