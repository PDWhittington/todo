using Todo.Contracts.Services;

namespace Todo.Service
{
    public class TodoService : ITodoService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICommandProvider _stateProvider;
        private readonly ITemplateProvider _templateProvider;

        public TodoService(IConfigurationProvider configurationProvider,
            ICommandProvider stateProvider, ITemplateProvider templateProvider)
        {
            _configurationProvider = configurationProvider;
            _stateProvider = stateProvider;
            _templateProvider = templateProvider;
        }
        
        public void PerformTask()
        {
            var configuration = _configurationProvider.GetConfiguration();
            var state = _stateProvider.GetCommand();

             

        }
    }
}