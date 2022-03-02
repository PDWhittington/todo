using System;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

public class ConfigurationInfo
{

    #region Paths to third-party tools

    public PerOsFilePaths BrowserLaunch { get; }

    public PerOsFilePaths TextEditorPath { get; }

    #endregion

    #region Paths to templates

    public string DayListMarkdownTemplatePath { get; }

    public string TopicListMarkdownTemplatePath { get; }

    public string HtmlTemplatePath { get; }

    #endregion

    #region Paths to folders the app writes to

    public string OutputFolder { get; }

    public string ArchiveFolderName { get; }

    #endregion

    public string TodoListFilenameFormat { get; }

    public bool UseGit { get; }

    public bool UseNamesForDays { get; }

    public TimeSpan? NewDayThreshold { get; }

    public int ConsoleWidth { get; }

    [JsonConstructor]
    public ConfigurationInfo(
        PerOsFilePaths browserLaunch, PerOsFilePaths textEditorPath,
        string dayListMarkdownTemplatePath, string topicListMarkdownTemplatePath,
        string htmlTemplatePath,
        string outputFolder, string archiveFolderName, string todoListFilenameFormat,
        bool useNamesForDays, bool useGit, TimeSpan? newDayThreshold, int consoleWidth)
    {
        BrowserLaunch = browserLaunch;
        TextEditorPath = textEditorPath;

        DayListMarkdownTemplatePath = dayListMarkdownTemplatePath;
        TopicListMarkdownTemplatePath = topicListMarkdownTemplatePath;
        HtmlTemplatePath = htmlTemplatePath;

        OutputFolder = outputFolder;
        ArchiveFolderName = archiveFolderName;
        TodoListFilenameFormat = todoListFilenameFormat;

        UseGit = useGit;
        UseNamesForDays = useNamesForDays;
        NewDayThreshold = newDayThreshold;
        ConsoleWidth = consoleWidth;
    }
}
