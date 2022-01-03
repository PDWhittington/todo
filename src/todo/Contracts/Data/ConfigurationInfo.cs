using System.Text.Json.Serialization;

namespace Todo.Contracts.Data
{
    public class ConfigurationInfo
    {
        public string BrowserPath { get; }

        public string TextEditorPath { get; }
        
        public string GitPath { get; }
        
        public string TemplatePath { get; }
        
        public bool UseNamesForDays { get; }
        
        public string OutputFolder { get; }
        
        public string ArchiveFolderName { get; }

        [JsonConstructor]
        public ConfigurationInfo(string templatePath, 
            bool useNamesForDays, string browserPath,
            string textEditorPath, string gitPath, string outputFolder, string archiveFolderName)
        {
            TemplatePath = templatePath;
            UseNamesForDays = useNamesForDays;
            BrowserPath = browserPath;
            TextEditorPath = textEditorPath;
            OutputFolder = outputFolder;
            ArchiveFolderName = archiveFolderName;
            GitPath = gitPath;
        }
    }
}