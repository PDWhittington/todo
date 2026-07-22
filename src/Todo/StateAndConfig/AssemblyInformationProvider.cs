using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class AssemblyInformationProvider : IAssemblyInformationProvider
{
    private readonly Assembly _executingAssembly = Assembly.GetExecutingAssembly();

    public string GetCommitHash()
    {
        var gitDescribe = GetMetadata("GitDescribe");
        return gitDescribe ?? throw new Exception("BuildTime not found");
    }

    public DateTime GetBuildTime()
    {
        var dteStr = GetMetadata("BuildTime");

        return dteStr is not null
            ? DateTime.Parse(dteStr)
            : throw new Exception("BuildTime not found");
    }
    
    public string? GetMetadata(string key)
    {
        return _executingAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == key)
            ?.Value;
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
    /// Returns the folder containing the executing assembly
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string GetAssemblyFolder()
        => Path.GetDirectoryName(AssemblyLocation()) ??
           throw new Exception("Cannot get containing folder of executing process");

    /// <summary>
    /// Returns the location of the executing assembly
    /// </summary>
    /// <returns></returns>
    public string AssemblyLocation() => Environment.ProcessPath! + ".dll";

    #if DEBUG
    public bool DebugFlag() => true;

    #else

    public bool DebugFlag() => false;

    #endif
}
