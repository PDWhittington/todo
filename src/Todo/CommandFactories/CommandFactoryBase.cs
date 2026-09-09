using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Exceptions;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

public abstract class CommandFactoryBase<T>(IConfigurationProvider configurationProvider,
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter, IEnumerable<string> wordsForCommand)
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

    protected bool IsThisCommand(CommandLineInfo commandLine)
    {
        if (!CommandWords.Contains(commandLine.Command))
            return false;

        OutputWriter.WriteLine($"Command line interpreted as {typeof(T).Name}");

        var excludedCommands = configurationProvider.ConfigInfo.Configuration.DisabledCommands;

        if (excludedCommands.Any(ec => wordsForCommand.Contains(ec.ToLower())))
        {
            var message = consoleTextFormatter.FormatWithForegroundColour("However, this command has been disabled.", ConsoleColor.Red);

            OutputWriter.WriteLine(message);
            throw new CommandExcludedException();
        }

        var otherWords = CommandWords
            .Where(word => !string.Equals(word, commandLine.Command, StringComparison.OrdinalIgnoreCase))
            .Select(word => $"'{word}'")
            .ToArray();

        if (otherWords.Length > 0)
        {
            OutputWriter.WriteLine($"(Can also be invoked with {string.Join(", ", otherWords)})");
        }

        OutputWriter.WriteLine();
        return true;
    }
}
