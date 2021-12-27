using System;
using System.Reflection;
using Todo.Contracts.Services;

namespace Todo.Path;

public class PathHelper : IPathHelper
{
    public string GetAssemblyFolder()
        => System.IO.Path.GetDirectoryName(GetAssemblyLocation()) ?? 
           throw new Exception("Cannot get containing folder of executing process");

    public string GetAssemblyLocation() => Assembly.GetEntryAssembly()?.Location;
}