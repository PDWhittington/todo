using System;
using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public class SettingsPathProvider : ISettingsPathProvider
{
    private readonly IPathHelper _pathHelper;
    private readonly IConstantsProvider _constantsProvider;
    private readonly Lazy<FilePathInfo> _settingsPath;

    public SettingsPathProvider(IPathHelper pathHelper, IConstantsProvider constantsProvider)
    {
        _pathHelper = pathHelper;
        _constantsProvider = constantsProvider;
        _settingsPath = new Lazy<FilePathInfo>(GetSettingsPath);
    }

    public FilePathInfo GetSettingsPathInHierarchy() => _settingsPath.Value;

    private static IEnumerable<string> GetAncestorsIncludingSelf(string folder)
    {
        var currentFolder = new DirectoryInfo(folder);

        if (!currentFolder.Exists) throw new DirectoryNotFoundException(folder);

        yield return currentFolder.FullName;

        while (currentFolder.Parent != null)
        {
            currentFolder = currentFolder.Parent;
            yield return currentFolder.FullName;
        }
    }

    private FilePathInfo GetSettingsPath()
    {
        var folderCandidates = GetAncestorsIncludingSelf(_pathHelper.GetWorkingFolder());

        foreach (var folderCandidate in folderCandidates)
        {
            var pathCandidate = GetSettingsPathInFolder(folderCandidate);

            if (pathCandidate.Exists()) return pathCandidate;
        }

        throw new Exception($"Cannot find {_constantsProvider.SettingsFileName}");
    }

    private FilePathInfo GetSettingsPathInFolder(string folder)
    {
        var path = Path.Combine(folder, _constantsProvider.SettingsFileName);
        return FilePathInfo.Of(path, FileTypeEnum.Settings, FolderEnum.AssemblyFolder);
    }

    public FilePathInfo GetSettingsPathInWorkingFolder()
        => GetSettingsPathInFolder(_pathHelper.GetWorkingFolder());
}
