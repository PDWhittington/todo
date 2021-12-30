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

        [JsonConstructor]
        public ConfigurationInfo(string templatePath, 
            bool useNamesForDays, string browserPath,
            string textEditorPath, string gitPath, string outputFolder)
        {
            TemplatePath = templatePath;
            UseNamesForDays = useNamesForDays;
            BrowserPath = browserPath;
            TextEditorPath = textEditorPath;
            OutputFolder = outputFolder;
            GitPath = gitPath;
        }
    }
}