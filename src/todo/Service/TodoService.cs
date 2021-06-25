using Todo.Contracts.Services;

namespace Todo.Service
{
    public class TodoService : ITodoService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IStateProvider _stateProvider;
        private readonly ITemplateProvider _templateProvider;

        public TodoService(IConfigurationProvider configurationProvider,
            IStateProvider stateProvider, ITemplateProvider templateProvider)
        {
            _configurationProvider = configurationProvider;
            _stateProvider = stateProvider;
            _templateProvider = templateProvider;
        }
        
        public void PerformTask()
        {
            var configuration = _configurationProvider.Configuration;
            var state = _stateProvider.GetState();

             

        }
    }
}