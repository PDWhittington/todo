namespace Todo.Contracts.Data
{
    public class ConfigurationInfo
    {
        public FilePath TemplatePath { get; }

        private ConfigurationInfo(FilePath templatePath)
        {
            TemplatePath = templatePath;
        }

        public static ConfigurationInfo Of(FilePath templatePath) => new(templatePath);
        
        public static ConfigurationInfo Of(string templatePath) => Of(new FilePath(templatePath));
    }
}