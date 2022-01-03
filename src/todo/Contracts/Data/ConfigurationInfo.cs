using System.Text.Json.Serialization;

namespace Todo.Contracts.Data;

public class ConfigurationInfo
{
    public string BrowserPath { get; }

    public string TextEditorPath { get; }

    public string GitPath { get; }

    public bool UseGit { get; }

    public string MarkdownTemplatePath { get; }

    public string HtmlTemplatePath { get; }

    public bool UseNamesForDays { get; }

    public string OutputFolder { get; }

    public string ArchiveFolderName { get; }

    [JsonConstructor]
    public ConfigurationInfo(string markdownTemplatePath, string htmlTemplatePath,
        bool useNamesForDays,
        string browserPath, string textEditorPath,
        string gitPath, bool useGit,
        string outputFolder, string archiveFolderName)
    {
        MarkdownTemplatePath = markdownTemplatePath;
        HtmlTemplatePath = htmlTemplatePath;
        UseNamesForDays = useNamesForDays;
        BrowserPath = browserPath;
        TextEditorPath = textEditorPath;
        OutputFolder = outputFolder;
        ArchiveFolderName = archiveFolderName;
        GitPath = gitPath;
        UseGit = useGit;
    }
}
