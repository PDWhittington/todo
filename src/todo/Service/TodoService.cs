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
using Todo.DateNaming;

namespace Todo.Service
{
    public class TodoService : ITodoService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICommandProvider _stateProvider;
        private readonly ITemplateProvider _templateProvider;
        private readonly IDateNamer _dateNamer;
        private readonly IGitInterface _gitInterface;

        public TodoService(IConfigurationProvider configurationProvider,
            ICommandProvider stateProvider, ITemplateProvider templateProvider,
            IDateNamer dateNamer, IGitInterface gitInterface)
        {
            _configurationProvider = configurationProvider;
            _stateProvider = stateProvider;
            _templateProvider = templateProvider;
            _dateNamer = dateNamer;
            _gitInterface = gitInterface;
        }
        
        public void PerformTask()
        {
            var command = _stateProvider.GetCommand();

            switch (command)
            {
                case CreateOrShowCommand createOrShowCommand:

                    CreateOrShow(createOrShowCommand);
                    break;
                
                case SyncCommand syncCommand:
                    Sync(syncCommand);
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

        private void Sync(SyncCommand syncCommand)
        {
            var configuration = _configurationProvider.GetConfiguration();

            var commitMessage = syncCommand.CommitMessage ?? $"Synced as at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            
            _gitInterface.RunGitCommand("reset");
            _gitInterface.RunGitCommand($"add \"{configuration.OutputFolder}\"");
            _gitInterface.RunGitCommand($"commit -m \"{commitMessage}\"");
            _gitInterface.RunGitCommand($"push");
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