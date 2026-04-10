using System;
using System.Text.Json.Serialization;
using Todo.Contracts.Data.Html;

namespace Todo.Contracts.Data.Config;

    public record Configuration(
        PerOsFilePaths BrowserPath, PerOsFilePaths TextEditorPath,
        string DayListMarkdownTemplatePath, string TopicListMarkdownTemplatePath,
        string HtmlTemplatePath, HtmlThemeEnum HtmlTheme,
        string OutputFolder, string ArchiveFolderName, string TodoListFilenameFormat,
        bool UseNamesForDays, bool UseGit, TimeSpan? NewDayThreshold, int ConsoleWidth,
        IterationMethodEnum FileIterationMethod);
