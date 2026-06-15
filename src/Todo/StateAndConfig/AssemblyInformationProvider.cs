using System;
using System.IO;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class AssemblyInformationProvider : IAssemblyInformationProvider
{
    private readonly IConstantsProvider _constantsProvider;
    private readonly IManifestStreamProvider _manifestStreamProvider;

    public AssemblyInformationProvider(IConstantsProvider constantsProvider, 
        IManifestStreamProvider manifestStreamProvider)
    {
        _constantsProvider = constantsProvider;
        _manifestStreamProvider = manifestStreamProvider;
    }

    public string GetCommitHash() =>
        _manifestStreamProvider.GetStringFromManifest(_constantsProvider.CommitHash.FullName).Trim();

    public DateTime GetBuildTime()
    {
        var dteStr = _manifestStreamProvider
            .GetStringFromManifest(_constantsProvider.BuildTime.FullName)
            .Trim();

        return DateTime.Parse(dteStr);
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
