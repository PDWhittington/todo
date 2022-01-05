using System;
using System.Collections.Generic;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.CommandFactories;
using Todo.Contracts.Services.DateParsing;

namespace Todo.CommandFactories;

public class PrintHtmlCommandFactory : CommandFactoryBase<PrintHtmlCommand>
{
    private readonly IDateParser _dateParser;

    public override bool IsDefaultCommandFactory => false;

    public override HashSet<string> WordsForCommand { get; }
        = new(StringComparer.InvariantCultureIgnoreCase)
        {
            "printhtml",
            "print",
            "p"
        };

    public PrintHtmlCommandFactory(IDateParser dateParser)
    {
        _dateParser = dateParser;
    }

    public override PrintHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintHtmlCommand.Of(dateOnly);
    }
}
