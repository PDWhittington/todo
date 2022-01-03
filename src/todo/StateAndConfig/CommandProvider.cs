using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class CommandProvider : ICommandProvider
{
    private readonly ICommandLineProvider _commandLineProvider;
    private readonly ICommandIdentifier _commandIdentifier;
    private readonly IDateParser _dateParser;

    public CommandProvider(ICommandLineProvider commandLineProvider,
        ICommandIdentifier commandIdentifier, IDateParser dateParser)
    {
        _commandLineProvider = commandLineProvider;
        _commandIdentifier = commandIdentifier;
        _dateParser = dateParser;
    }

    public CommandBase GetCommand()
    {
        if (_commandIdentifier.TryGetCommandType(out var commandType, out var restOfCommand))
        {
            switch (commandType)
            {
                case ICommandIdentifier.CommandTypeEnum.Archive:
                {
                    if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
                        throw new ArgumentException("Date in archive command is not recognised");

                    return ArchiveCommand.Of(dateOnly);
                }

                case ICommandIdentifier.CommandTypeEnum.Commit:

                    return CommitCommand.Of(restOfCommand);

                case ICommandIdentifier.CommandTypeEnum.Push:

                    return PushCommand.Singleton;

                case ICommandIdentifier.CommandTypeEnum.ShowHtml:
                {
                    if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
                        throw new ArgumentException("Date in archive command is not recognised");

                    return ShowHtmlCommand.Of(dateOnly);
                }

                case ICommandIdentifier.CommandTypeEnum.Sync:

                    return SyncCommand.Of(restOfCommand);

                default: throw new Exception("Command not yet implemented.");
            }
        }

        var commandLine = _commandLineProvider.GetCommandLineMinusAssemblyLocation();

        if (_dateParser.TryGetDate(commandLine, out var date))
        {
            return CreateOrShowCommand.Of(date);
        }

        throw new Exception("Command not recognised");
    }
}
