using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services;

namespace Todo.State
{
    public class CommandProvider : ICommandProvider
    {
        private readonly ICommandLineParser _commandLineParser;

        public CommandProvider(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }
        
        public CommandBase GetCommand()
        {
            var commandLine = _commandLineParser.GetCommandLineMinusAssemblyLocation();

            if (commandLine.StartsWith("sync", StringComparison.CurrentCultureIgnoreCase))
            {
                var commitMessage = commandLine.Substring("sync".Length).Trim();

                if (string.IsNullOrWhiteSpace(commitMessage)) commitMessage = null;
                return SyncCommand.Of(commitMessage);
            }
            
            var date = _commandLineParser.GetDateFromCommandLine();
            return CreateOrShowCommand.Of(date);
        }
    }
}