using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Services.Dates.Parsing;

namespace Todo.Dates.Parsing;

public class DateHelper : IDateHelper
{
    public DateOnly ConvertToDateOnly(DateTime dateTime)
        => new(dateTime.Year, dateTime.Month, dateTime.Day);

    public DateTime ConvertToDateTime(DateOnly dateTime)
        => dateTime.ToDateTime(new TimeOnly(0, 0, 0));

    public int DateDiff(DateOnly a, DateOnly b)
        => (int)(ConvertToDateTime(a) - ConvertToDateTime(b)).TotalDays;

    public DateOnly GetNearestTo(IEnumerable<DateOnly> candidates, DateOnly to)
        => candidates
            .OrderBy(x => AbsoluteDateDiff(x, to))
            .First();

    public int AbsoluteDateDiff(DateOnly a, DateOnly b)
        => Math.Abs(DateDiff(a, b));

    public bool TryGetNthOfPreviousMonth(DateOnly currentDay, int n, out DateOnly nOfMonth)
    {
        if (currentDay.Month == 1 && n <= DaysInMonth(12, currentDay.Year - 1))
        {
            nOfMonth = new DateOnly(currentDay.Year - 1, 12, n);
        }
        else if (currentDay.Month != 1 && n <= DaysInMonth(currentDay.Month - 1, currentDay.Year))
        {
            nOfMonth = new DateOnly(currentDay.Year, currentDay.Month - 1, n);
        }
        else
        {
            nOfMonth = default;
        }

        return nOfMonth != default;
    }

    public bool TryGetNthOfCurrentMonth(DateOnly currentDay, int n, out DateOnly nOfMonth)
    {
        nOfMonth = n <= DaysInMonth(currentDay.Month, currentDay.Year) ?
            new DateOnly(currentDay.Year, currentDay.Month, n) :
            default;

        return nOfMonth != default;
    }

    public bool TryGetNthOfNextMonth(DateOnly currentDay, int n, out DateOnly nOfMonth)
    {
        if (currentDay.Month == 12 && n <= DaysInMonth(1, currentDay.Year))
        {
            nOfMonth = new DateOnly(currentDay.Year + 1, 01, n);
        }
        else if (currentDay.Month != 12 && n <= DaysInMonth(currentDay.Month + 1, currentDay.Year))
        {
            nOfMonth = new DateOnly(currentDay.Year, currentDay.Month + 1, n);
        }
        else
        {
            nOfMonth = default;
        }

        return nOfMonth != default;
    }

    public bool TryGetDateInPreviousYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear)
        => TryGetDate(currentDay.Year - 1, month, day, out dateInYear);

    public bool TryGetDateInCurrentYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear)
        => TryGetDate(currentDay.Year, month, day, out dateInYear);

    public bool TryGetDateInFollowingYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear)
        => TryGetDate(currentDay.Year, month, day, out dateInYear);

    private static bool TryGetDate(int year, int month, int day, out DateOnly dateInYear)
    {
        var daysInMonth = DaysInMonth(month, year);

        if (day < 0 || day > daysInMonth)
        {
            dateInYear = default;
            return false;
        }

        dateInYear = new DateOnly(year, month, day);
        return true;
    }

    private static int DaysInMonth(int month, int year)
        => month == 12 ? 31 :
        new DateOnly(year, month + 1, 1).AddDays(-1).Day;
}
