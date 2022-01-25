using System;
using System.Collections.Generic;

namespace Todo.Contracts.Services.Helpers;

public interface IDateHelper
{
    DateOnly ConvertToDateOnly(DateTime dateTime);

    // ReSharper disable once UnusedMemberInSuper.Global
    DateTime ConvertToDateTime(DateOnly dateTime);

    // ReSharper disable once UnusedMemberInSuper.Global
    int DateDiff(DateOnly a, DateOnly b);

    DateOnly GetNearestTo(IEnumerable<DateOnly> candidates, DateOnly to);

    // ReSharper disable once UnusedMemberInSuper.Global
    int AbsoluteDateDiff(DateOnly a, DateOnly b);

    bool TryGetNthOfPreviousMonth(DateOnly currentDay, int n, out DateOnly nOfMonth);

    bool TryGetNthOfCurrentMonth(DateOnly currentDay, int n, out DateOnly nOfMonth);

    bool TryGetNthOfNextMonth(DateOnly currentDay, int n, out DateOnly nOfMonth);

    bool TryGetDateInPreviousYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear);

    bool TryGetDateInCurrentYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear);

    bool TryGetDateInFollowingYear(DateOnly currentDay, int month, int day, out DateOnly dateInYear);
}
