using System;

namespace Todo.Contracts.Data.Commands;

public class CreateOrShowDayListCommand : CreateOrShowCommandBase
{
    public DateOnly Date { get; }

    private CreateOrShowDayListCommand(DateOnly date)
    {
        Date = date;
    }

    public static CreateOrShowDayListCommand Of(DateOnly date) => new(date);
}
