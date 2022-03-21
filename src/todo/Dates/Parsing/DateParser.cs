using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Contracts.Services.Dates.Parsing;
using IConfigurationProvider = Todo.Contracts.Services.StateAndConfig.IConfigurationProvider;

namespace Todo.Dates.Parsing;

public class DateParser : IDateParser
{
    private readonly IDateHelper _dateHelper;
    private readonly IConfigurationProvider _configurationProvider;

    public DateParser(IDateHelper dateHelper, IConfigurationProvider configurationProvider)
    {
        _dateHelper = dateHelper;
        _configurationProvider = configurationProvider;
    }

    public bool TryGetDate(string? str, out DateOnly dateOnly)
    {
        if (str == null) return false;

        //NOTE: order of these tests is important.

        if (IsYesterday(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(-1);
        else if (IsToday(str)) dateOnly = GetTodayWithMidnightAdjusted();
        else if (IsTomorrow(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(1);
        else if (IsRelativeOffset(str, out var offset)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(offset);
        else if (IsDayOfWeek(str, out var dayOfWeek)) dateOnly = GetDateFromDayOfWeek((DayOfWeek)dayOfWeek!);
        else if (IsDayOnly(str, out var day)) dateOnly = GetDateFromDayOnly(day);
        else if (IsLastThisOrNext(str, out var dateFromColloquial)
                 && dateFromColloquial != null) dateOnly = dateFromColloquial.Value;
        else if (IsDayMonthOnly(str, out day, out var month)) dateOnly = GetDateFromDayMonth(month, day);

        else if (DateOnly.TryParse(str, out var dte)) dateOnly = dte;
        else dateOnly = default;

        return dateOnly != default;
    }

    private DateOnly GetTodayWithMidnightAdjusted()
    {
        var newDayThreshold = _configurationProvider.ConfigInfo.Configuration.NewDayThreshold ?? new TimeSpan(0, 0, 0);

        return DateTime.Now.TimeOfDay < newDayThreshold
            ? _dateHelper.ConvertToDateOnly(DateTime.Today.AddDays(-1))
            : _dateHelper.ConvertToDateOnly(DateTime.Today);
    }

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

    private static bool IsDayOfWeek(string commandLine, out DayOfWeek? dayOfWeek)
    {
        switch (commandLine.ToLower())
        {
            case "sun":
            case "sunday":
                dayOfWeek = DayOfWeek.Sunday; return true;

            case "mon":
            case "monday":
                dayOfWeek = DayOfWeek.Monday; return true;

            case "tue":
            case "tuesday":
                dayOfWeek = DayOfWeek.Tuesday; return true;

            case "wed":
            case "wednesday":
                dayOfWeek = DayOfWeek.Wednesday; return true;

            case "thu":
            case "thursday":
                dayOfWeek = DayOfWeek.Thursday; return true;

            case "fri":
            case "friday":
                dayOfWeek = DayOfWeek.Friday; return true;

            case "sat":
            case "saturday":
                dayOfWeek = DayOfWeek.Saturday; return true;

            default:
                dayOfWeek = default; return false;
        }
    }

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

    private bool IsLastThisOrNext(string commandLine, out DateOnly? date)
    {
        bool FirstWordIsKey(string firstWord)
            => "last".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase) ||
               "this".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase) ||
               "next".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase);

        var elements = commandLine.Split(' ');

        if (elements.Length == 2 &&
            FirstWordIsKey(elements[0]) &&
            IsDayOfWeek(elements[1], out var dayOfWeek) &&
            dayOfWeek != null /* for compiler */)
        {
            var currentDate = GetTodayWithMidnightAdjusted();

            var dateDiffs = GetDateDiffsFor(currentDate, dayOfWeek.Value);

            var diffToApply = "last".Equals(elements[0], StringComparison.CurrentCultureIgnoreCase)
                ? dateDiffs.Where(x => x < 0).Max()
                : dateDiffs.Where(x => x > 0).Min();

            date = currentDate.AddDays(diffToApply);
            return true;
        }

        date = null;
        return false;
    }

    private DateOnly GetDateFromDayOfWeek(DayOfWeek dayOfWeek)
    {
        var today = GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayOfWeek(today, dayOfWeek);

        if (possibles.Length == 0) throw new Exception($"No dates found for day = {dayOfWeek}");
        return _dateHelper.GetNearestTo(possibles, today);
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

    private static DateOnly[] GetPossiblesForDayOfWeek(DateOnly currentDay, DayOfWeek dayOfWeek)
    {
        var dateDiffs = GetDateDiffsFor(currentDay, dayOfWeek);

        return dateDiffs
            .Select(currentDay.AddDays)
            .ToArray();
    }




    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    private static int[] GetDateDiffsFor(DateOnly currentDay, DayOfWeek dayOfWeek)
    {
        var currentDayIndex = MapDayOfWeekToNumber(currentDay.DayOfWeek);
        var dayOfWeekIndex = MapDayOfWeekToNumber(dayOfWeek);

        return new []
        {
            dayOfWeekIndex - 7 - currentDayIndex,
            dayOfWeekIndex - currentDayIndex,
            dayOfWeekIndex + 7 - currentDayIndex
        };
    }

    private static int MapDayOfWeekToNumber(DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Sunday => 0,
        DayOfWeek.Monday => 1,
        DayOfWeek.Tuesday => 2,
        DayOfWeek.Wednesday => 3,
        DayOfWeek.Thursday => 4,
        DayOfWeek.Friday => 5,
        DayOfWeek.Saturday => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
    };
}
