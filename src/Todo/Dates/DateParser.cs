using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Dates;

public class DateParser(IConfigurationProvider configurationProvider, 
    IDateHelper dateHelper, IDateAdjuster dateAdjuster, IEnvironmentVariableProvider environmentVariableProvider) : IDateParser
{

    public bool TryGetDate(string? str, out DateOnly dateOnly)
    { 
        return EnvironmentVariableSetsOverride(out var overrideDate) 
            ? TryGetDateRelativeTo(str, overrideDate, out dateOnly) 
            : TryGetDateRelativeTo(str, dateAdjuster.GetTodayWithMidnightAdjusted(), out dateOnly);
    }

    private bool TryGetDateRelativeTo(string? str, DateOnly relativeToDate, out DateOnly dateOnly)
    {
        //NOTE: order of these tests is important.

        if (str is null) dateOnly = default;
        else if (IsYesterday(str)) dateOnly = relativeToDate.AddDays(-1);
        else if (IsToday(str)) dateOnly = relativeToDate;
        else if (IsTomorrow(str)) dateOnly = relativeToDate.AddDays(1);
        else if (IsRelativeOffset(str, out var offset)) dateOnly = relativeToDate.AddDays(offset);
        else if (IsDayOfWeek(str, out var dayOfWeek)) dateOnly = GetDateFromDayOfWeek((DayOfWeek)dayOfWeek!);
        else if (IsDayOnly(str, out var day)) dateOnly = GetDateFromDayOnly(day);
        else if (IsLastThisOrNext(str, out var dateFromColloquial)
                 && dateFromColloquial is not null) dateOnly = dateFromColloquial.Value;
        else if (IsDayMonthOnly(str, out day, out var month)) dateOnly = GetDateFromDayMonth(month, day);

        else if (DateOnly.TryParse(str, out var dte)) dateOnly = dte;
        else dateOnly = default;

        return dateOnly != default;
    }

    private bool EnvironmentVariableSetsOverride(out DateOnly overrideDate)
    {
        var environmentVariableName = configurationProvider.ConfigInfo
            .Configuration.EnvironmentVariableToOverrideDate;

        if (!string.IsNullOrWhiteSpace(environmentVariableName)
            && environmentVariableProvider.TryGetEnvironmentVariable(environmentVariableName, out var overrideDateStr))
        {
            return TryGetDateRelativeTo(overrideDateStr, dateAdjuster.GetTodayWithMidnightAdjusted(), out overrideDate);
        }

        overrideDate = default;
        return false;
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
                dayOfWeek = null; return false;
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
                    offset = 0;
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

        if (elements.Length != 2)
        {
            day = 0;
            month = 0;
            return false;
        }

        var dayParsed = int.TryParse(elements[0], out day);
        var monthParsed = int.TryParse(elements[1], out month);

        return dayParsed && monthParsed;
    }

    private bool IsLastThisOrNext(string commandLine, out DateOnly? date)
    {
        var elements = commandLine.Split(' ');

        if (elements.Length == 2 &&
            FirstWordIsKey(elements[0]) &&
            IsDayOfWeek(elements[1], out var dayOfWeek) &&
            dayOfWeek is not null /* for compiler */)
        {
            var currentDate = dateAdjuster.GetTodayWithMidnightAdjusted();

            var dateDiffs = GetDateDiffsFor(currentDate, dayOfWeek.Value);

            var diffToApply = "last".Equals(elements[0], StringComparison.CurrentCultureIgnoreCase)
                ? dateDiffs.Where(x => x < 0).Max()
                : dateDiffs.Where(x => x > 0).Min();

            date = currentDate.AddDays(diffToApply);
            return true;
        }

        date = null;
        return false;

        bool FirstWordIsKey(string firstWord)
            => "last".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase) ||
               "this".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase) ||
               "next".Equals(firstWord, StringComparison.CurrentCultureIgnoreCase);
    }

    private DateOnly GetDateFromDayOfWeek(DayOfWeek dayOfWeek)
    {
        var today = dateAdjuster.GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayOfWeek(today, dayOfWeek);

        return possibles.Length != 0 
            ? dateHelper.GetNearestTo(possibles, today)
            : throw new Exception($"No dates found for day = {dayOfWeek}");

    }

    private DateOnly GetDateFromDayOnly(int dayOnly)
    {
        var today = dateAdjuster.GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayOnly(today, dayOnly).ToArray();

        return possibles.Length == 0
            ? dateHelper.GetNearestTo(possibles, today)
            : throw new Exception($"No dates found for day = {dayOnly}");
    }

    private DateOnly GetDateFromDayMonth(int month, int day)
    {
        var today = dateAdjuster.GetTodayWithMidnightAdjusted();

        var possibles = GetPossiblesForDayMonth(today, month, day);
        return dateHelper.GetNearestTo(possibles, today);
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    [SuppressMessage("ReSharper", "DuplicatedSequentialIfBodies")]
    private DateOnly [] GetPossiblesForDayOnly(DateOnly currentDay, int n)
    {
        return [.. PotentialDates()];

        IEnumerable<DateOnly> PotentialDates()
        {
            if (dateHelper.TryGetNthOfPreviousMonth(currentDay, n, out var nOfMonth)) yield return nOfMonth;
            if (dateHelper.TryGetNthOfCurrentMonth(currentDay, n, out nOfMonth)) yield return nOfMonth;
            if (dateHelper.TryGetNthOfNextMonth(currentDay, n, out nOfMonth)) yield return nOfMonth;
        }
    }

    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    [SuppressMessage("ReSharper", "DuplicatedSequentialIfBodies")]
    private DateOnly [] GetPossiblesForDayMonth(DateOnly currentDay, int month, int day)
    {
        return [.. PotentialDates()];

        IEnumerable<DateOnly> PotentialDates()
        {
            if (dateHelper.TryGetDateInPreviousYear(currentDay, month, day, out var nOfMonth)) yield return nOfMonth;
            if (dateHelper.TryGetDateInCurrentYear(currentDay, month, day, out nOfMonth)) yield return nOfMonth;
            if (dateHelper.TryGetDateInFollowingYear(currentDay, month, day, out nOfMonth)) yield return nOfMonth;
        }
    }

    private static DateOnly[] GetPossiblesForDayOfWeek(DateOnly currentDay, DayOfWeek dayOfWeek)
    {
        var dateDiffs = GetDateDiffsFor(currentDay, dayOfWeek);

        return
        [
            .. dateDiffs
                .Select(currentDay.AddDays)
        ];
    }




    // ReSharper disable once ReturnTypeCanBeEnumerable.Local
    private static int[] GetDateDiffsFor(DateOnly currentDay, DayOfWeek dayOfWeek)
    {
        var currentDayIndex = MapDayOfWeekToNumber(currentDay.DayOfWeek);
        var dayOfWeekIndex = MapDayOfWeekToNumber(dayOfWeek);

        return
        [
            dayOfWeekIndex - 7 - currentDayIndex,
            dayOfWeekIndex - currentDayIndex,
            dayOfWeekIndex + 7 - currentDayIndex
        ];
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