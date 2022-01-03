using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Helpers;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class SettingsPathProvider : ISettingsPathProvider
{
    private readonly IPathHelper _pathHelper;

    public SettingsPathProvider(IPathHelper pathHelper)
    {
        _pathHelper = pathHelper;
    }

    public FilePathInfo GetSettingsPath()
    {
        var path = System.IO.Path.Combine(_pathHelper.GetAssemblyFolder(), "settings.json");
        return FilePathInfo.Of(path, FileTypeEnum.Settings, FolderEnum.ProgramRoot);
    }
}
