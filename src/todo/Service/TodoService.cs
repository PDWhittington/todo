using System;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
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
            var command = _stateProvider.GetCommand();

            switch (command)
            {
                case CreateOrShowCommand createOrShowCommand:

                    CreateOrShow(createOrShowCommand);
                    break;
                
                default:
                    throw new Exception();
            }
        }

        private void CreateOrShow(CreateOrShowCommand createOrShowCommand)
        {
            var configuration = _configurationProvider.GetConfiguration();
            
            var fileName = $"todo-{createOrShowCommand.Date:yyyy-MM-dd}.md";
            var path = System.IO.Path.Combine(configuration.OutputFolder, fileName);

            if (!File.Exists(path))
            {
                var templateText = _templateProvider.GetTemplate();
                
                
                
            }
        }

        private string GetDateText(ConfigurationInfo configurationInfo, DateOnly date)
        {
            
        }
    }
}