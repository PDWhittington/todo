namespace Todo.Contracts.Exceptions;

public class SettingsNotFoundException(string[] pathsWhereSettingsNotFound)
    : TodoExceptionBase("Settings file not found")
{
    public override string Advice()
    {
        return $"Todo has searched for todo-settings.json in the following locations:- {Environment.NewLine}{Environment.NewLine}\t" +
               string.Join(Environment.NewLine + "\t", pathsWhereSettingsNotFound) +
               $"{Environment.NewLine}{Environment.NewLine}Consider running todo init to create a default settings file.";
    }
}
