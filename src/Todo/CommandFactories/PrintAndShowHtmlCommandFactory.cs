using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PrintAndShowHtmlCommandFactory(IDateParser dateParser, IConfigurationProvider configurationProvider, 
    IConsoleTextFormatter consoleTextFormatter, IOutputWriter outputWriter)
    : CommandFactoryBase<PrintAndShowHtmlCommand>(configurationProvider, consoleTextFormatter, outputWriter, Words)
{
    private static readonly string[] Words = ["ph", "printandshowhtml"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        "This command is equivalent to printhtml followed by showhtml (p, h)."
    ];

    protected override string Usage => "ph [date]";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override PrintAndShowHtmlCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        if (!dateParser.TryGetDate(commandLine.Switches, out var dateOnly))
        {
            throw new ArgumentException("Date in archive command is not recognised");
        }

        return PrintAndShowHtmlCommand.Of(dateOnly);
    }
}
