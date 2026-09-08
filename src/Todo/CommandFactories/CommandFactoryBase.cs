using System;
using System.Collections.Generic;
using Todo.Contracts.Data.CommandLine;
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

    public abstract T? TryGetCommand(CommandLineInfo commandLine);

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


    protected bool IsThisCommand(CommandLineInfo commandLine) => CommandWords.Contains(commandLine.Command);
}
