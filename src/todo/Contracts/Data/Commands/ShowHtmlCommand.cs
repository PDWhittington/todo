using System;

namespace Todo.Contracts.Data.Commands;

public class ShowHtmlCommand : CommandBase
{
    public DateOnly Date { get; }

    private ShowHtmlCommand(DateOnly date)
    {
        Date = date;
    }

    public static ShowHtmlCommand Of(DateOnly date) => new(date);
}