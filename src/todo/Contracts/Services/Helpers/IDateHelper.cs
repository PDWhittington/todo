using System;

namespace Todo.Contracts.Services.Helpers;

public interface IDateHelper
{
    DateOnly ConvertToDateOnly(DateTime dateTime);
    
    DateTime ConvertToDateTime(DateOnly dateTime);

    int DateDiff(DateOnly a, DateOnly b);

    int AbsoluteDateDiff(DateOnly a, DateOnly b);

    public bool TryGetNthOfPreviousMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth);

    public bool TryGetNthOfCurrentMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth);

    public bool TryGetNthOfNextMonth(DateOnly currentDay, int n, out DateOnly? nOfMonth);
}