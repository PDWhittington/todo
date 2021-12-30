using System;
using System.Reflection;
using Todo.Contracts.Services;

namespace todo.CommandLine
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
        
        public DateOnly GetDateFromCommandLine()
        {
            var commandLine = GetCommandLineMinusAssemblyLocation();
            
            if (IsYesterday(commandLine)) return GetTodayWithMidnightAdjusted().AddDays(-1);
            if (IsToday(commandLine)) return GetTodayWithMidnightAdjusted();
            if (IsTomorrow(commandLine)) return GetTodayWithMidnightAdjusted().AddDays(1);
            if (IsRelativeOffset(commandLine, out var offset)) return GetTodayWithMidnightAdjusted().AddDays(offset);
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

        private bool IsRelativeOffset(string commandLine, out int offset)
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
                        throw new ArgumentException("unrecognised control character before number", nameof(commandLine));        
                }
            }

            offset = int.MinValue;
            return false;
        }

        private bool IsDayOnly(string commandLine, out int dayOnly) => int.TryParse(commandLine, out dayOnly);

        private DateOnly GetDateFromDayOnly(int dayOnly)
        {
            var today = GetTodayWithMidnightAdjusted();
            
            //Case where day is coming up in the current month
            if (today.Day <= dayOnly) return new DateOnly(dayOnly, today.Month, today.Year);

            //Case where day has passed in the current month and it's not December
            if (today.Month < 12) return new DateOnly(dayOnly, today.Month + 1, today.Year);
            
            //Case where day has passed in the current month and it is December
            return new DateOnly(dayOnly, 1, today.Year);
        }
        
        private string GetCommandLineMinusAssemblyLocation()
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