using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.StateAndConfig;

public interface ISettingsPathProvider
{
    FilePathInfo? SettingsPathInHierarchy { get; }

    FilePathInfo GetSettingsPathInFolder(string folder);

    FilePathInfo GetSettingsPathInWorkingFolder();
}
