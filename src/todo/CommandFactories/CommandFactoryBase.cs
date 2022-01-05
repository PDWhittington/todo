using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.CommandFactories;

public abstract class CommandFactoryBase
{
    public abstract HashSet<string> WordsForCommand { get; }

    protected bool IsThisCommand(string commandLine, out string? restOfCommand)
    {
        var firstWord = FirstWordToLower(commandLine);

        if (!WordsForCommand.Contains(firstWord))
        {
            restOfCommand = null;
            return false;
        }

        restOfCommand = commandLine[firstWord.Length..].Trim();
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
