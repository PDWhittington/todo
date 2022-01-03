using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class EasterDateNamer : IEasterDateNamer
{
    public bool TryGetSpecialName(DateOnly date, out string? name)
    {
        var easterDay = EasterDayForYear(date.Year);

        var dayDiff = DayDifference(date, easterDay);

        name = dayDiff switch
        {
            1 => "Easter Monday",
            0 => "Easter Day",
            -1 => "Holy Saturday",
            -2 => "Good Friday",
            -3 => "Maundy Thursday",
            _ => null
        };

        return name != null;
    }

    private static int DayDifference(DateOnly date, DateOnly easterDay)
    {
        var diff = date.ToDateTime(new TimeOnly(0L)) - easterDay.ToDateTime(new TimeOnly(0L));
        return (int)diff.TotalDays;
    }

    private static DateOnly EasterDayForYear(int year)
    {
        var d = 225 - 11 * (year % 19);

        var dMult30 = d > 50 ? (int)Math.Ceiling((d - 50) / 30.0) : 0;

        d -= dMult30 * 30;

        if (d > 48) d--;

        var e = (year + year / 4 + d + 1) % 7;

        var q = d + 7 - e;

        return new DateOnly(year, 3, 1).AddDays(q - 1);
    }
}
