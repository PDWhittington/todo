using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.DateParsing;

namespace Todo.CommandFactories;

public class CreateOrShowCommandFactory : CommandFactoryBase<CreateOrShowCommand>
{
    private readonly IDateParser _dateParser;

    public override bool IsDefaultCommandFactory => true;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "createorshow"
        };

    public CreateOrShowCommandFactory(IDateParser dateParser)
    {
        _dateParser = dateParser;
    }

    public override CreateOrShowCommand? TryGetCommand(string commandLine)
    {
        var commandLineToUse = IsThisCommand(commandLine, out var restOfCommand)
            ? restOfCommand : commandLine;

        if (!_dateParser.TryGetDate(commandLineToUse, out var dateOnly))
        {
            throw new Exception("Date not recognised in command");
        }

        return CreateOrShowCommand.Of(dateOnly);
    }
}
