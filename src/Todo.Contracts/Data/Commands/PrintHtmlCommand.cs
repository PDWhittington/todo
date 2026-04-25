namespace Todo.Contracts.Data.Commands;

public record PrintHtmlCommand : CommandBase
{
    public DateOnly Date { get; }

    private PrintHtmlCommand(DateOnly date)
    {
        Date = date;
    }

    public static PrintHtmlCommand Of(DateOnly date) => new(date);
}
