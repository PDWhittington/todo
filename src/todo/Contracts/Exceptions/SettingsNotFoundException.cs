using System;

namespace Todo.Contracts.Exceptions;

public class SettingsNotFoundException : TodoExceptionBase
{
    private readonly string[] _pathsWhereSettingsNotFound;

    public SettingsNotFoundException(string[] pathsWhereSettingsNotFound)
        : base("Settings file not found")
    {
        _pathsWhereSettingsNotFound = pathsWhereSettingsNotFound;
    }

    public override string Advice()
    {
        return $"Todo has searched for todo-settings.json in the following locations:- {Environment.NewLine}{Environment.NewLine}\t" +
               string.Join(Environment.NewLine + "\t", _pathsWhereSettingsNotFound) +
               $"{Environment.NewLine}{Environment.NewLine}Consider running todo init to create a default settings file.";
    }
}
