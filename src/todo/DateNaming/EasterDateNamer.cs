using System;

namespace Todo.DateNaming;

public class EasterDateNamer : IEasterDateNamer
{
    /*
     * Calculate D="'225'" - 11(Y MOD 19).

If D is greater than 50 then subtract multiples of 30 until the resulting new value of D is

less than 51.

If D is greater than 48 subtract 1 from it.

Calculate E="'(Y" +' [Y/4] + D + 1) MOD 7. (NB Integer part of [Y/4])

Calculate Q="'D +'" 7 - E.

If Q is less than 32 then Easter is in March. If Q is greater than 31 then Q - 31 is its date in April.

For example, for 1998:

D = 225 - 11*(1998 MOD 19) = 225 - 11*3 = 192

D is greater than 50, therefore:

D = (192 - 5*30) = 42

E = (1998 + [1998/4] + 42 + 1) MOD 7="'2540'" MOD 7="'6'"

Q = 42 + 7 - 6="'43'"

Easter 1998="'43" -' 31="'12" April'

That’s nice and simple then!
     */
    
    public bool TryGetName(DateOnly date, out string name)
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