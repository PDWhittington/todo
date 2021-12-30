using System;

namespace Todo.DateNaming;

public class ChristmasNewYearDateNamer : IChristmasNewYearDateNamer
{
    public bool TryGetName(DateOnly date, out string name)
    {
        name = date switch
        {
            { Month: 1, Day: 1 } => "New Year's Day",
            { Month: 1, Day: 6 } => "Epiphany",
            { Month: 12, Day: 24 } => "Christmas Eve",
            { Month: 12, Day: 25 } => "Christmas Day",
            { Month: 12, Day: 26 } => "Boxing Day",
            { Month: 12, Day: 31 } => "New Year's Eve",
            _ => null
        };

        return name != null;
    }
}