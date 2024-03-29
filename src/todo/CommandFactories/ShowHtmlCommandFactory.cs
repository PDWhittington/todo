﻿using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates.Parsing;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHtmlCommandFactory : CommandFactoryBase<ShowHtmlCommand>
{
    private static readonly string[] Words = { "h", "html", "showhtml" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } = {
        "Opens the browser specified in the settings file and loads the Html file for the given date.",
        "",
        "Usage: todo h [date]"
    };

    private readonly IDateParser _dateParser;

    public ShowHtmlCommandFactory(IDateParser dateParser, IOutputWriter outputWriter)
        : base(outputWriter, Words)
    {
        _dateParser = dateParser;
    }

    public override ShowHtmlCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!_dateParser.TryGetDate(restOfCommand, out var dateOnly))
            throw new ArgumentException("Date in archive command is not recognised");

        return ShowHtmlCommand.Of(dateOnly);
    }
}
