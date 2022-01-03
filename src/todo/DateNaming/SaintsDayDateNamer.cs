using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class SaintsDayDateNamer : ISaintsDayDateNamer
{
    public bool TryGetSpecialName(DateOnly date, out string? name)
    {
        name = date switch
        {
            { Month: 03, Day: 01 } => "St. David's Day",
            { Month: 03, Day: 17 } => "St. Patrick's Day",
            { Month: 04, Day: 23 } => "St. George's Day",
            { Month: 11, Day: 30 } => "St. Andrew's Day",
            _ => null
        };

        return name != null;
    }
}
