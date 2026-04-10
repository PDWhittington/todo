using System;
using System.Text.Json.Serialization;
using Todo.Contracts.Data.Html;

namespace Todo.Contracts.Data.Config;

public record Configuration
{
    #region Paths to third-party tools

    public PerOsFilePaths BrowserPath { get; }

    public PerOsFilePaths TextEditorPath { get; }

    #endregion

    #region Paths to templates

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string DayListMarkdownTemplatePath { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string TopicListMarkdownTemplatePath { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string HtmlTemplatePath { get; }
    
    #endregion

    public HtmlThemeEnum HtmlTheme { get; }
    
    #region Paths to folders the app writes to

    public string OutputFolder { get; }

    public string ArchiveFolderName { get; }

    #endregion

    public string TodoListFilenameFormat { get; }

    public bool UseGit { get; }

    public bool UseNamesForDays { get; }

    public TimeSpan? NewDayThreshold { get; }

    public int ConsoleWidth { get; }
    
    public IterationMethodEnum FileIterationMethod { get; }

    [JsonConstructor]
    public Configuration(
        PerOsFilePaths browserPath, PerOsFilePaths textEditorPath,
        string dayListMarkdownTemplatePath, string topicListMarkdownTemplatePath,
        string htmlTemplatePath, HtmlThemeEnum htmlTheme,
        string outputFolder, string archiveFolderName, string todoListFilenameFormat,
        bool useNamesForDays, bool useGit, TimeSpan? newDayThreshold, int consoleWidth,
        IterationMethodEnum fileIterationMethod)
    {
        BrowserPath = browserPath;
        TextEditorPath = textEditorPath;

        DayListMarkdownTemplatePath = dayListMarkdownTemplatePath;
        TopicListMarkdownTemplatePath = topicListMarkdownTemplatePath;
        HtmlTemplatePath = htmlTemplatePath;
        HtmlTheme = htmlTheme;

        OutputFolder = outputFolder;
        ArchiveFolderName = archiveFolderName;
        TodoListFilenameFormat = todoListFilenameFormat;

        UseGit = useGit;
        UseNamesForDays = useNamesForDays;
        NewDayThreshold = newDayThreshold;
        ConsoleWidth = consoleWidth;
        FileIterationMethod = fileIterationMethod;
    }
}
