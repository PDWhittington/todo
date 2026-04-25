using System;

namespace Todo.Contracts.Data.Commands;

public record ShowHtmlCommand : CommandBase
{
    public DateOnly Date { get; }

    private ShowHtmlCommand(DateOnly date)
    {
        Date = date;
    }

    public static ShowHtmlCommand Of(DateOnly date) => new(date);
}