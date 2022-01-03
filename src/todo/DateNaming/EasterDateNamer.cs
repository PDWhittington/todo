using System;
using Todo.Contracts.Services.DateNaming;

namespace Todo.DateNaming;

public class EasterDateNamer : IEasterDateNamer
{
    public bool TryGetSpecialName(DateOnly date, out string name)
    {
        var easterDay = EasterDayForYear(date.Year);

        var dayDiff = DayDifference(date, easterDay);

        switch (dayDiff)
        {
            case 1:
                name = "Easter Monday"; break;
            case 0:
                name = "Easter Day"; break;
            case -1:
                name = "Holy Saturday"; break;
            case -2:
                name = "Good Friday"; break;
            case -3:
                name = "Maundy Thursday"; break;
            default:
                name = null; break;
        }

        return name != null;
    }

    private int DayDifference(DateOnly date, DateOnly easterDay)
    {
        var diff = date.ToDateTime(new TimeOnly(0L)) - easterDay.ToDateTime(new TimeOnly(0L));
        return (int)diff.TotalDays;
    }

    private DateOnly EasterDayForYear(int year)
    {
        var d = 225 - 11 * (year % 19);

        var dMult30 = d > 50 ? (int)Math.Ceiling((double)(d - 50) / 30.0) : 0;

        d -= dMult30 * 30;

        if (d > 48) d--;

        var e = (year + year / 4 + d + 1) % 7;

        var q = d + 7 - e;

        return new DateOnly(year, 3, 1).AddDays(q - 1);
    }
}