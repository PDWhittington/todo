namespace Todo.Contracts.Data.Commands;

public record CreateOrShowDayListCommand : CreateOrShowCommandBase
{
    public DateOnly Date { get; }

    private CreateOrShowDayListCommand(DateOnly date)
    {
        Date = date;
    }

    public static CreateOrShowDayListCommand Of(DateOnly date) => new(date);
}
