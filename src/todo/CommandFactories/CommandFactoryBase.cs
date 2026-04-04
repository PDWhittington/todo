using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

public abstract class CommandFactoryBase<T>(IOutputWriter outputWriter, IEnumerable<string> wordsForCommand)
    : ICommandFactory<T>
    where T : CommandBase
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly IOutputWriter OutputWriter = outputWriter;

    public abstract T? TryGetCommand(string commandLine);

    public abstract bool IsDefaultCommandFactory { get; }

    public abstract string [] HelpText { get; }

    public HashSet<string> CommandWords { get; } = new(wordsForCommand, StringComparer.InvariantCultureIgnoreCase);

    protected bool IsThisCommand(string commandLine, out string? restOfCommand)
    {
        var firstWord = FirstWordToLower(commandLine);

        if (!CommandWords.Contains(firstWord))
        {
            restOfCommand = null;
            return false;
        }

        restOfCommand = commandLine[firstWord.Length..].Trim();

        OutputWriter.WriteLine($"Command line interpreted as {typeof(T).Name}");
        OutputWriter.WriteLine();

        return true;
    }

    private static string FirstWordToLower(string str)
    {
        var index = str.IndexOf(' ');

        return index switch
        {
            -1 or 0 => str,
            _ => str[..index].ToLower()
        };
    }
}
