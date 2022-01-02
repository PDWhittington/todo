using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Todo.Contracts.Services;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig
{
    /// <summary>
    /// This class 
    /// </summary>
    public class CommandLineParser : ICommandLineParser
    {
        private readonly IDateHelper _dateHelper;

        public CommandLineParser(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
        }

        public bool TryGetWordFromCommandLine(string[] candidates, out string word)
        {
            var commandLine = GetCommandLineMinusAssemblyLocation();

            word = candidates.FirstOrDefault(x =>
                string.Equals(x, commandLine, StringComparison.CurrentCultureIgnoreCase));

            return word != default;
        }
        
        public DateOnly GetDateFromCommandLine()
        {
            var commandLine = GetCommandLineMinusAssemblyLocation();
            
            if (IsYesterday(commandLine)) return GetTodayWithMidnightAdjusted().AddDays(-1);
            if (IsToday(commandLine)) return GetTodayWithMidnightAdjusted();
            if (IsTomorrow(commandLine)) return GetTodayWithMidnightAdjusted().AddDays(1);
            if (IsRelativeOffset(commandLine, out var offset)) return GetTodayWithMidnightAdjusted().AddDays((int)offset);
            if (IsDayOnly(commandLine, out var dayOnly)) return GetDateFromDayOnly(dayOnly);

            if (DateOnly.TryParse(commandLine, out var dte)) return dte;

            throw new Exception("date is not a recognised format");
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
                switch (commandLine[1])
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

        public string GetCommandLineMinusAssemblyLocation()
        {
            var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? "";

            var wholeCommandLine = Environment.CommandLine;
            
            if (wholeCommandLine.StartsWith(assemblyLocation))
            {
                return wholeCommandLine
                    .Substring(assemblyLocation.Length)
                    .Trim();
            }

            return wholeCommandLine.Trim();
        }
    }
}