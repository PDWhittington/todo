using System;

namespace Todo.Contracts.Data.Commands;

public class PrintAndShowHtmlCommand : CommandBase
{
    public DateOnly Date { get; }

    private PrintAndShowHtmlCommand(DateOnly date)
    {
        Date = date;
    }

    public static PrintAndShowHtmlCommand Of(DateOnly date) => new(date);
}
