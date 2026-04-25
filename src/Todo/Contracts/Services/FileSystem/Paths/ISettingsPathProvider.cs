using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem.Paths;

public interface ISettingsPathProvider
{
    FilePathInfo GetSettingsPathInHierarchy();

    FilePathInfo GetSettingsPathInWorkingFolder();
}
