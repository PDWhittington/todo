using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services;
using Todo.DateNaming;

namespace Todo.Service
{
    public class TodoService : ITodoService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICommandProvider _stateProvider;
        private readonly ITemplateProvider _templateProvider;
        private readonly IDateNamer _dateNamer;

        public TodoService(IConfigurationProvider configurationProvider,
            ICommandProvider stateProvider, ITemplateProvider templateProvider,
            IDateNamer dateNamer)
        {
            _configurationProvider = configurationProvider;
            _stateProvider = stateProvider;
            _templateProvider = templateProvider;
            _dateNamer = dateNamer;
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
            var path = Path.Combine(configuration.OutputFolder, fileName);

            if (!File.Exists(path))
            {
                var templateText = _templateProvider.GetTemplate();

                var outputText = templateText.Replace(
                    "{date}", GetDateText(configuration, createOrShowCommand.Date));
                
                File.WriteAllText(path, outputText);
            }

            Process.Start(configuration.TextEditorPath, path);
        }

        private string GetDateText(ConfigurationInfo configurationInfo, DateOnly date)
        {
            if (configurationInfo.UseNamesForDays &&
                _dateNamer.TryGetName(date, out var dateName))
            {
                return $"{dateName}, {date.Year}";
            }

            return $"{date:Ddd} <sup>{GetOrdinal(date.Day)}</sup> {date:Mmmm}, {date:YYYY}";
        }

        private string GetOrdinal(int num)
        {
            if (num < 1 || num > 31) throw new ArgumentException("Out of range", nameof(num));

            return num switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                21 => "st",
                22 => "nd",
                23 => "rd",
                31 => "st",
                _ => "th"
            };
        }
    }
}