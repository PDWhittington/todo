using System;
using System.Collections.Generic;
using System.Linq;
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

    protected abstract string [] HelpText { get; }

    protected abstract string Usage { get; }

    public IEnumerable<string> GetFullHelpMessage()
    {
        if (HelpText.Length == 0) yield break;

        foreach (var helpText in HelpText) yield return helpText;

        yield return "";
        yield return $"Usage: todo {Usage}";
    }

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

        var otherWords = CommandWords
            .Where(word => !string.Equals(word, firstWord))
            .Select(word => $"'{word}'")
            .ToArray();

        OutputWriter.WriteLine($"Command line interpreted as {typeof(T).Name}");

        if (otherWords.Length > 0)
        {
            OutputWriter.WriteLine($"(Can also be invoked with {string.Join(", ", otherWords)})");
        }

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