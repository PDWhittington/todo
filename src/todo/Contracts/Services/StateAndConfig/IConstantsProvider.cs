namespace Todo.Contracts.Services.StateAndConfig;

public interface IConstantsProvider
{
    string SettingsFileName { get; }

    string SettingsManifestResourceStream { get; }
}
