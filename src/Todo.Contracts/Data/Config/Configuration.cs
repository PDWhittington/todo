using System.Text.Json.Serialization;
using Todo.Contracts.Data.Html;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record Configuration(TodoListInfo TodoListInfo,
    PerOsLaunchInfos BrowserPath, PerOsLaunchInfos TextEditorPath,
    string DayListMarkdownTemplatePath, string TopicListMarkdownTemplatePath,
    string EnvironmentVariableToOverrideDate,
    string HtmlTemplatePath, HtmlThemeEnum HtmlTheme,
    string OutputFolder, string ArchiveFolderName, string TodoListFilenameFormatWithoutExension,
    bool UseNamesForDays, bool UseGit, TimeSpan? NewDayThreshold, int ConsoleWidth,
    IterationMethodEnum FileIterationMethod, int DefaultDayIntervalForGamify, ScoreCategory[] ScoreCategories);