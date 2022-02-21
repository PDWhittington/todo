using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class SettingsPathProvider : ISettingsPathProvider
{
    private const string _settingsFileName = "todo-settings.json";

    private readonly IPathHelper _pathHelper;
    private FilePathInfo? _settingsPath;

    public SettingsPathProvider(IPathHelper pathHelper)
    {
        _pathHelper = pathHelper;
    }

    public FilePathInfo SettingsPath => _settingsPath ?? GetSettingsPath();

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

    private FilePathInfo GetSettingsPath()
    {
        var folderCandidates = GetAncestorsIncludingSelf(_pathHelper.GetWorkingFolder());

        foreach (var folderCandidate in folderCandidates)
        {
            var pathCandidate = Path.Combine(folderCandidate, "todo-settings.json");

            if (!File.Exists(pathCandidate)) continue;

            _settingsPath = FilePathInfo.Of(pathCandidate, FileTypeEnum.Settings, FolderEnum.ProgramRoot);
            return _settingsPath.Value;
        }

        throw new FileNotFoundException($"Cannot find settings file with name {_settingsFileName}");
    }
}
