using Todo.Contracts.Data;
using Todo.Contracts.Services;

namespace Todo.State
{
    public class StateProvider : IStateProvider
    {
        private readonly ICommandLineParser _commandLineParser;

        public StateProvider(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }
        
        public StateInfo GetState()
        {
            var date = _commandLineParser.GetDateFromCommandLine();
            return StateInfo.Of(date);
        }
    }
}