using System;
using Todo.Contracts.Services;

namespace Todo.Helpers;

public class DateHelper : IDateHelper
{
    public DateOnly ConvertToDateOnly(DateTime dateTime) 
        => new(dateTime.Year, dateTime.Month, dateTime.Day);

    public DateTime ConvertToDateTime(DateOnly dateTime)
        => dateTime.ToDateTime(new TimeOnly(0, 0, 0));

    public int DateDiff(DateOnly a, DateOnly b) 
        => (int)(ConvertToDateTime(a) - ConvertToDateTime(b)).TotalDays;
    
    public int AbsoluteDateDiff(DateOnly a, DateOnly b) 
        => Math.Abs(DateDiff(a, b));

    public bool TryGetNthOfPreviousMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth)
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

    public bool TryGetNthOfCurrentMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth)
    {
        nOfMonth = n <= DaysInMonth(currentDay.Month, currentDay.Year) ? 
            new DateOnly(currentDay.Year, currentDay.Month, n) : 
            default(DateOnly?);

        return nOfMonth != default;
    }

    public bool TryGetNthOfNextMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth)
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

    public int DaysInMonth(int month, int year)
        => month == 12 ? 31 : 
        new DateOnly(year, month + 1, 1).AddDays(-1).Day;

    private DateOnly GetNthOfPreviousMonth(DateOnly currentDay, int n)
        => currentDay.Month == 1
            ? new DateOnly(currentDay.Year - 1, 12, n)
            : new DateOnly(currentDay.Year, currentDay.Month - 1, n);

    private DateOnly GetNthOfCurrentMonth(DateOnly currentDay, int n)
        => new DateOnly(currentDay.Year, currentDay.Month, n);
        
    private DateOnly GetNthOfNextMonth(DateOnly currentDay, int n)
        => currentDay.Month == 12
            ? new DateOnly(currentDay.Year + 1, 1, n)
            : new DateOnly(currentDay.Year, currentDay.Month + 1, n);
}