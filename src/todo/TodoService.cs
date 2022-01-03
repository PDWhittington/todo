using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;
using Todo.Execution;

namespace Todo
{
    public class TodoService : ITodoService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICommandProvider _commandProvider;
        private readonly ITemplateProvider _templateProvider;
        private readonly IDateNamer _dateNamer;
        private readonly ISyncExecutor _syncExecutor;

        public TodoService(IConfigurationProvider configurationProvider,
            ICommandProvider commandProvider, ITemplateProvider templateProvider,
            IDateNamer dateNamer, ISyncExecutor syncExecutor)
        {
            _configurationProvider = configurationProvider;
            _commandProvider = commandProvider;
            _templateProvider = templateProvider;
            _dateNamer = dateNamer;
            _syncExecutor = syncExecutor;
        }
        
        public void PerformTask()
        {
            var command = _commandProvider.GetCommand();

            switch (command)
            {
                case CreateOrShowCommand createOrShowCommand:

                    CreateOrShow(createOrShowCommand);
                    break;
                
                case SyncCommand syncCommand:
                    _syncExecutor.Execute(syncCommand);
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
            
            return $"{date:dddd d}<sup>{GetOrdinal(date.Day)}</sup> {date:MMMM}, {date:yyyy}";
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