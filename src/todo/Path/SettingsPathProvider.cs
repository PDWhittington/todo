using Todo.Contracts.Services;

namespace Todo.Path;

public class SettingsPathProvider : ISettingsPathProvider
{
    private readonly IPathHelper _pathHelper;

    public SettingsPathProvider(IPathHelper pathHelper)
    {
        _pathHelper = pathHelper;
    }

    public string GetSettingsPath() 
        => System.IO.Path.Combine(_pathHelper.GetAssemblyFolder(), "settings.json");
}