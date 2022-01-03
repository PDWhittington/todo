using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ITemplateProvider _templateProvider;
    private readonly IDateNamer _dateNamer;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, ITemplateProvider templateProvider, IDateNamer dateNamer)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _dateNamer = dateNamer;
    }

    public void Execute(CreateOrShowCommand createOrShowCommand)
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