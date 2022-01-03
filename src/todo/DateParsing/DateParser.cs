using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.Helpers;

namespace Todo.DateParsing;

public class DateParser : IDateParser
{
    private readonly IDateHelper _dateHelper;

    public DateParser(IDateHelper dateHelper)
    {
        _dateHelper = dateHelper;
    }

    public bool TryGetDate(string? str, out DateOnly dateOnly)
    {
        if (str == null) return false;

        if (IsYesterday(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(-1);
        else if (IsToday(str)) dateOnly = GetTodayWithMidnightAdjusted();
        else if (IsTomorrow(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(1);
        else if (IsRelativeOffset(str, out var offset)) dateOnly = GetTodayWithMidnightAdjusted()
                                                                        .AddDays(offset);
        else if (IsDayOnly(str, out var day)) dateOnly = GetDateFromDayOnly(day);
        else if (IsDayMonthOnly(str, out day, out var month)) dateOnly = GetDateFromDayMonth(month, day);
        else if (DateOnly.TryParse(str, out var dte)) dateOnly = dte;
        else dateOnly = default;

        return dateOnly != default;
    }

     private DateOnly GetTodayWithMidnightAdjusted()
            => DateTime.Now.TimeOfDay < new TimeSpan(04, 00, 00)
            ? _dateHelper.ConvertToDateOnly(DateTime.Today.AddDays(-1))
            : _dateHelper.ConvertToDateOnly(DateTime.Today);

    private static bool IsYesterday(string commandLine) => commandLine.ToLower() switch
    {
        "y" => true,
        "yesterday" => true,
        _ => false
    };

    private static bool IsToday(string commandLine) =>
        string.IsNullOrWhiteSpace(commandLine) ||
        ".".Equals(commandLine) ||
        "today".Equals(commandLine.ToLower());

    private static bool IsTomorrow(string commandLine) => commandLine.ToLower() switch
    {
        "tm" => true,
        "tomorrow" => true,
        _ => false
    };

    private static bool IsRelativeOffset(string commandLine, out int offset)
    {
        if (int.TryParse(commandLine[1..], out var parsed))
        {
            switch (commandLine[0])
            {
                case '+':
                    offset = parsed;
                    return true;
                case '-':
                    offset = -parsed;
                    return true;
                default:
                    offset = default;
                    return false;

            }
        }

        offset = int.MinValue;
        return false;
    }

    private static bool IsDayOnly(string commandLine, out int dayOnly)
        => int.TryParse(commandLine, out dayOnly) && dayOnly is > 0 and < 32;

    private static bool IsDayMonthOnly(string commandLine, out int day, out int month)
    {
        var elements = commandLine.Split('/', '.', '-');

        var dayParsed = int.TryParse(elements[0], out day);
        var monthParsed = int.TryParse(elements[1], out month);

        return
            elements.Length == 2 && dayParsed && monthParsed;
    }

    private DateOnly GetDateFromDayOnly(int dayOnly)
    {
        var today = GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayOnly(today, dayOnly).ToArray();

        if (possibles.Length == 0) throw new Exception($"No dates found for day = {dayOnly}");
        return _dateHelper.GetNearestTo(possibles, today);
    }

    private DateOnly GetDateFromDayMonth(int month, int day)
    {
        var today = GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayMonth(today, month, day);
        return _dateHelper.GetNearestTo(possibles, today);
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    private DateOnly [] GetPossiblesForDayOnly(DateOnly currentDay, int n)
    {
        IEnumerable<DateOnly> PotentialDates()
        {
            if (_dateHelper.TryGetNthOfPreviousMonth(currentDay, n, out var nOfMonth)) yield return nOfMonth;
            if (_dateHelper.TryGetNthOfCurrentMonth(currentDay, n, out nOfMonth)) yield return nOfMonth;
            if (_dateHelper.TryGetNthOfNextMonth(currentDay, n, out nOfMonth)) yield return nOfMonth;
        }

        return PotentialDates().ToArray();
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    private DateOnly [] GetPossiblesForDayMonth(DateOnly currentDay, int month, int day)
    {
        IEnumerable<DateOnly> PotentialDates()
        {
            if (_dateHelper.TryGetDateInPreviousYear(currentDay, month, day, out var nOfMonth)) yield return nOfMonth;
            if (_dateHelper.TryGetDateInCurrentYear(currentDay, month, day, out nOfMonth)) yield return nOfMonth;
            if (_dateHelper.TryGetDateInFollowingYear(currentDay, month, day, out nOfMonth)) yield return nOfMonth;
        }

        return PotentialDates().ToArray();
    }
}
