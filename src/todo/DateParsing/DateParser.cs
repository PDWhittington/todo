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
    
    public bool TryGetDate(string str, out DateOnly? dateOnly)
    {
        if (IsYesterday(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(-1);
        else if (IsToday(str)) dateOnly = GetTodayWithMidnightAdjusted();
        else if (IsTomorrow(str)) dateOnly = GetTodayWithMidnightAdjusted().AddDays(1);
        else if (IsRelativeOffset(str, out var offset)) dateOnly = GetTodayWithMidnightAdjusted()
                                                                        .AddDays((int)offset);
        else if (IsDayOnly(str, out var dayOnly)) dateOnly = GetDateFromDayOnly(dayOnly);
        else if (DateOnly.TryParse(str, out var dte)) dateOnly = dte;
        else dateOnly = null;

        return dateOnly != default;
    }
    
     private DateOnly GetTodayWithMidnightAdjusted() 
            => DateTime.Now.TimeOfDay < new TimeSpan(04, 00, 00) 
            ? _dateHelper.ConvertToDateOnly(DateTime.Today.AddDays(-1)) 
            : _dateHelper.ConvertToDateOnly(DateTime.Today);

        private bool IsYesterday(string commandLine)=> commandLine.ToLower() switch
        {
            "y" => true,
            "yesterday" => true,
            _ => false,
        };
        
        private bool IsToday(string commandLine) => 
            string.IsNullOrWhiteSpace(commandLine) ||
            ".".Equals(commandLine) ||
            "today".Equals(commandLine.ToLower());

        private bool IsTomorrow(string commandLine) => commandLine.ToLower() switch
        {
            "tm" => true,
            "tomorrow" => true,
            _ => false,
        };

        private bool IsRelativeOffset(string commandLine, out int? offset)
        {
            if (int.TryParse(commandLine.Substring(1), out var parsed))
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

        private bool IsDayOnly(string commandLine, out int dayOnly) 
            => int.TryParse(commandLine, out dayOnly) && dayOnly is > 0 and < 32;

        private DateOnly GetDateFromDayOnly(int dayOnly)
        {
            var today = GetTodayWithMidnightAdjusted();

            var possibles = GetPossibles(today, dayOnly);

            var nearestDate = possibles
                .OrderBy(x => _dateHelper.AbsoluteDateDiff(x, today))
                .First();

            return nearestDate;
        }

        IEnumerable<DateOnly> GetPossibles(DateOnly currentDay, int n)
        {
            if (_dateHelper.TryGetNthOfPreviousMonth(currentDay, n, out var nOfMonth)) yield return (DateOnly)nOfMonth;
            if (_dateHelper.TryGetNthOfCurrentMonth(currentDay, n, out nOfMonth)) yield return (DateOnly)nOfMonth;
            if (_dateHelper.TryGetNthOfNextMonth(currentDay, n, out nOfMonth)) yield return (DateOnly)nOfMonth;
        }
}