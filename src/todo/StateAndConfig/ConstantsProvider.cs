using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConstantsProvider : IConstantsProvider
{
    public string SettingsFileName => "todo-settings.json";

    public string SettingsManifestResourceStream => $"Todo.{SettingsFileName}";
}
