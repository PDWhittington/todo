using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

public abstract class CommandFactoryBase<T> : ICommandFactory<T> where T : CommandBase
{
    protected readonly IOutputWriter _outputWriter;
    public abstract T? TryGetCommand(string commandLine);

    public abstract bool IsDefaultCommandFactory { get; }

    public abstract string [] HelpText { get; }

    public HashSet<string> CommandWords { get; }

    protected CommandFactoryBase(IOutputWriter outputWriter, IEnumerable<string> wordsForCommand)
    {
        _outputWriter = outputWriter;
        CommandWords = new HashSet<string>(wordsForCommand, StringComparer.InvariantCultureIgnoreCase);
    }

    protected bool IsThisCommand(string commandLine, out string? restOfCommand)
    {
        var firstWord = FirstWordToLower(commandLine);

        if (!CommandWords.Contains(firstWord))
        {
            restOfCommand = null;
            return false;
        }

        restOfCommand = commandLine[firstWord.Length..].Trim();

        _outputWriter.WriteLine($"Command line interpreted as {typeof(T).Name}");
        _outputWriter.WriteLine();

        return true;
    }

    private static string FirstWordToLower(string str)
    {
        var index = str.IndexOf(" ", StringComparison.Ordinal);

        return index switch
        {
            -1 => str,
            0 => str,
            _ => str[..index].ToLower()
        };
    }
}
