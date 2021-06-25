namespace Todo.Contracts.Data
{
    public class ConfigurationInfo
    {
        public string TemplatePath { get; }

        private ConfigurationInfo(string templatePath)
        {
            TemplatePath = templatePath;
        }

        public static ConfigurationInfo Of(string templatePath) => new(templatePath);
    }
}