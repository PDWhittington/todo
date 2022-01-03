using System;
using System.Linq;
using System.Reflection;
using Todo.Contracts.Services.DateParsing;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig
{
    /// <summary>
    /// This class 
    /// </summary>
    public class CommandLineProvider : ICommandLineProvider
    {
        private readonly IDateParser _dateParser;

        public CommandLineProvider(IDateParser dateParser)
        {
            _dateParser = dateParser;
        }

        public bool TryGetWordFromCommandLine(string[] candidates, out string word)
        {
            var commandLine = GetCommandLineMinusAssemblyLocation();

            word = candidates.FirstOrDefault(x =>
                string.Equals(x, commandLine, StringComparison.CurrentCultureIgnoreCase));

            return word != default;
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