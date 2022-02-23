using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class SettingsPathProvider : ISettingsPathProvider
{
    private readonly IPathHelper _pathHelper;
    private readonly IConstantsProvider _constantsProvider;
    private FilePathInfo? _settingsPath;

    public SettingsPathProvider(IPathHelper pathHelper, IConstantsProvider constantsProvider)
    {
        _pathHelper = pathHelper;
        _constantsProvider = constantsProvider;
    }

    public FilePathInfo? SettingsPathInHierarchy => _settingsPath ?? GetSettingsPath();

    private IEnumerable<string> GetAncestorsIncludingSelf(string folder)
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

    private FilePathInfo? GetSettingsPath()
    {
        var folderCandidates = GetAncestorsIncludingSelf(_pathHelper.GetWorkingFolder());

        foreach (var folderCandidate in folderCandidates)
        {
            var pathCandidate = Path.Combine(folderCandidate, _constantsProvider.SettingsFileName);

            if (!File.Exists(pathCandidate)) continue;

            _settingsPath = FilePathInfo.Of(pathCandidate, FileTypeEnum.Settings, FolderEnum.ProgramRoot);
        }

        return _settingsPath;
    }

    public FilePathInfo GetSettingsPathInFolder(string folder)
    {
        var path = Path.Combine(folder, _constantsProvider.SettingsFileName);
        return FilePathInfo.Of(path, FileTypeEnum.Settings, FolderEnum.ProgramRoot);
    }

    public FilePathInfo GetSettingsPathInWorkingFolder()
        => GetSettingsPathInFolder(_pathHelper.GetWorkingFolder());
}
