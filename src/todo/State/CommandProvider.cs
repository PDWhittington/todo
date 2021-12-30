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
            var date = _commandLineParser.GetDateFromCommandLine();
            return CreateOrShowCommand.Of(date);
        }
    }
}