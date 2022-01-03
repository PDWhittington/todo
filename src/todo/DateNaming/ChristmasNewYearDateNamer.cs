using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class ChristmasNewYearDateNamer : IChristmasNewYearDateNamer
{
    public bool TryGetSpecialName(DateOnly date, out string name)
    {
        name = date switch
        {
            { Month: 12, Day: 24 } => "Christmas Eve",
            { Month: 12, Day: 25 } => "Christmas Day",
            { Month: 12, Day: 26 } => "Boxing Day",
            { Month: 12, Day: 31 } => "New Year's Eve",
            { Month: 1, Day: 1 } => "New Year's Day",
            { Month: 1, Day: 6 } => "Epiphany",
            _ => null
        };

        return name != null;
    }
}