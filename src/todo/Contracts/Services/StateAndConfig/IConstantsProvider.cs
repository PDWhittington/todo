using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IConstantsProvider
{
    string SettingsFileName { get; }

    ManifestInfo DefaultSettingsFile { get; }

    ManifestInfo DayListMarkdownTemplate { get; }

    ManifestInfo TopicListMarkdownTemplate { get; }

    ManifestInfo DefaultHtmlTemplate { get; }

    ManifestInfo CommitHash { get; }
}
