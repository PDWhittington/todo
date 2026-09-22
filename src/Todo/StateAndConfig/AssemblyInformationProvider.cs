using System;
using System.Linq;
using System.Reflection;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class AssemblyInformationProvider(IFileSystemFactory fileSystemFactory)
    : IAssemblyInformationProvider
{
    private readonly Assembly _executingAssembly = Assembly.GetExecutingAssembly();
    private readonly IFileSystem _fileSystem = fileSystemFactory.Create();

    public PackageReferenceInfo[] GetPackageReferences()
    {
        var packageReferencesStr = GetMetadata("PackageReferences");

        if (packageReferencesStr is null || string.IsNullOrWhiteSpace(packageReferencesStr))
        {
            return [];
        }

        var packageReferenceList = packageReferencesStr
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var elements = line.Split('|', StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length != 3)
                    return null;

                var projetName = elements[0];
                var identity = elements[1];
                var version = elements[2];

                return new PackageReferenceInfo(projetName, identity, version);
            })
            .Where(x => x is not null)
            .Cast<PackageReferenceInfo>()
            .ToArray();

        return packageReferenceList;
    }

    public string GitDescribe()
    {
        var gitDescribe = GetMetadata("GitDescribe");
        return gitDescribe ?? throw new Exception("BuildTime not found");
    }

    public string[] GitBranches()
    {
        var gitBranches = GetMetadata("GitBranches");

        if (gitBranches is null || string.IsNullOrWhiteSpace(gitBranches))
            return [];

        var branchList = gitBranches
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Trim())
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .OrderBy(b =>
                string.Equals(b, "master", StringComparison.OrdinalIgnoreCase)
                || string.Equals(b, "main", StringComparison.OrdinalIgnoreCase)
                    ? 0
                    : 1
            )
            .ThenBy(b => b);

        return [.. branchList];
    }

    public string[] GitTags()
    {
        var gitTags = GetMetadata("GitTags");

        if (gitTags is null || string.IsNullOrWhiteSpace(gitTags))
            return [];

        var branchList = gitTags
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Trim())
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .OrderBy(b => b);

        return [.. branchList];
    }

    public string[] GitWorktreeChanges()
    {
        var gitWorktreeChanges = GetMetadata("GitWorktreeChanges");

        if (gitWorktreeChanges is null || string.IsNullOrWhiteSpace(gitWorktreeChanges))
            return [];

        var gitChangeList = gitWorktreeChanges
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(b => b.Trim())
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .OrderBy(b => b);

        return [.. gitChangeList];
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
        return _executingAssembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
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
        var rootedPath = _fileSystem.IsPathRooted(path)
            ? path
            : _fileSystem.Combine(GetAssemblyFolder(), path);
        return _fileSystem.GetFullPath(rootedPath);
    }

    /// <summary>
    /// Returns the folder containing the executing assembly
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string GetAssemblyFolder() =>
        _fileSystem.GetDirectoryName(AssemblyLocation())
        ?? throw new Exception("Cannot get containing folder of executing process");

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