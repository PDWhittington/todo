﻿using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates.Parsing;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowDayListCommandFactory : CommandFactoryBase<CreateOrShowDayListCommand>
{
    private static readonly string[] Words = { "createorshow" };

    public override bool IsDefaultCommandFactory => true;

    public override string[] HelpText { get; } = {
        "Creates or shows a markdown file for the date supplied. " +
        "This is the default command and can be executed by typing anything that can be parsed as a date. " +
        "Supplying no date assumes the current day.",
        "",
        "Usage: todo [date]"
    };

    private readonly IDateParser _dateParser;

    public CreateOrShowDayListCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
    {
        _dateParser = dateParser;
    }

    public override CreateOrShowDayListCommand TryGetCommand(string commandLine)
    {
        var commandLineToUse = IsThisCommand(commandLine, out var restOfCommand)
            ? restOfCommand : commandLine;

        if (!_dateParser.TryGetDate(commandLineToUse, out var dateOnly))
        {
            throw new Exception("Date not recognised in command");
        }

        return CreateOrShowDayListCommand.Of(dateOnly);
    }
}
