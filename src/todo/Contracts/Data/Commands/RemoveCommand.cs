using System;

namespace Todo.Contracts.Data.Commands;

public class RemoveCommand : CommandBase
{
    public DateOnly Date { get; }

    private RemoveCommand(DateOnly date)
    {
        Date = date;
    }

    public static RemoveCommand Of(DateOnly dateOfFileToRemove) => new(dateOfFileToRemove);
}
