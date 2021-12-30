using System.Text.Json.Serialization;

namespace Todo.Contracts.Data
{
    public class ConfigurationInfo
    {
        public string BrowserPath { get; }

        public string TextEditorPath { get; }
        
        public string TemplatePath { get; }
        
        public bool UseNamesForDays { get; }

        [JsonConstructor]
        public ConfigurationInfo(string templatePath, 
            bool useNamesForDays, string browserPath,
            string textEditorPath)
        {
            TemplatePath = templatePath;
            UseNamesForDays = useNamesForDays;
            BrowserPath = browserPath;
            TextEditorPath = textEditorPath;
        }
    }
}