using Todo.Contracts.Services;
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

    public string GetSettingsPath() 
        => System.IO.Path.Combine(_pathHelper.GetAssemblyFolder(), "settings.json");
}