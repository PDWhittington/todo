using System;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class CommandIdentifier : ICommandIdentifier
{
    private readonly ICommandLineProvider _commandLineProvider;

    public CommandIdentifier(ICommandLineProvider commandLineProvider)
    {
        _commandLineProvider = commandLineProvider;
    }

    public bool TryGetCommandType(out ICommandIdentifier.CommandTypeEnum? commandType, out string? restOfCommand)
    {
        var commandLine = _commandLineProvider.GetCommandLineMinusAssemblyLocation();

        var firstWord = FirstWordToLower(commandLine);

        switch (firstWord)
        {
            case "archive":
            case "a":
                commandType = ICommandIdentifier.CommandTypeEnum.Archive;
                break;

            case "commit":
            case "c":
                commandType = ICommandIdentifier.CommandTypeEnum.Commit;
                break;

            case "push":
            case "p":
                commandType = ICommandIdentifier.CommandTypeEnum.Push;
                break;

            case "showhtml":
            case "html":
            case "h":
                commandType = ICommandIdentifier.CommandTypeEnum.ShowHtml;
                break;

            case "sync":
            case "s":
                commandType = ICommandIdentifier.CommandTypeEnum.Sync;
                break;

            default:
                commandType = default;
                break;
        }

        restOfCommand = commandType != default ? commandLine[firstWord.Length..].Trim() : default;
        return commandType != default;
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
