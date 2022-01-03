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

    public bool TryGetCommandType(out ICommandIdentifier.CommandTypeEnum? commandName, out string restOfCommand)
    {
        var commandLine = _commandLineProvider.GetCommandLineMinusAssemblyLocation();

        var firstWord = this.firstWordToLower(commandLine);

        switch (firstWord)
        {
            case "sync":
            case "s":
                commandName = ICommandIdentifier.CommandTypeEnum.Sync;
                restOfCommand = commandLine.Substring(firstWord.Length).Trim();
                break;
            default:
                commandName = default;
                restOfCommand = default;
                break;
        }

        return commandName != default;
    }

    string firstWordToLower(string str)
    {
        var index = str.IndexOf(" ", StringComparison.Ordinal); 
        
        switch (index)
        {
            case -1: return str;
            case 0: return str;
            default:
                return str.Substring(0, index).ToLower();
        }
    }
}