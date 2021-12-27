using System.Text.Json.Serialization;

namespace Todo.Contracts.Data
{
    public class ConfigurationInfo
    {
        public string TemplatePath { get; }
        
        public bool UseNamesForDays { get; }

        [JsonConstructor]
        public ConfigurationInfo(string templatePath, bool useNamesForDays)
        {
            TemplatePath = templatePath;
            UseNamesForDays = useNamesForDays;
        }
    }
}