using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class ConstantsProvider : IConstantsProvider
{
    // ReSharper disable once InconsistentNaming
    private const string _settingsFileName = "todo-settings.json";

    public string SettingsFileName => _settingsFileName;

    public ManifestInfo DefaultSettingsFile { get; } = ManifestInfo.Of(_settingsFileName);
    
    public ManifestInfo DayListMarkdownTemplate { get; } = ManifestInfo.Of("day-list-template.md");
    
    public ManifestInfo TopicListMarkdownTemplate { get; } = ManifestInfo.Of("topic-list-template.md");
    
    public ManifestInfo DefaultHtmlTemplate { get; } = ManifestInfo.Of("template.html");
    
    public ManifestInfo CommitHash { get; } = ManifestInfo.Of("commithash.txt");
    
    public ManifestInfo BuildTime { get; } = ManifestInfo.Of("buildtime.txt");

    public string ProjectAuthor { get; } = "Phil Whittington";

    public string ProjectAuthorContactDetails { get; } = "Twitter: @PDWhittington, DMs open";


    public string ProjectWebsite { get; } = "https://github.com/PDWhittington/todo";

}
