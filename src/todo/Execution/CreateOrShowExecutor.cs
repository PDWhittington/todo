using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ITemplateProvider _templateProvider;
    private readonly ISpecialDateNamer _dateNamer;
    private readonly IFileNamer _fileNamer;
    private readonly ISubstitutionMaker _substitutionMaker;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, ITemplateProvider templateProvider, 
        ISpecialDateNamer dateNamer, IFileNamer fileNamer, ISubstitutionMaker substitutionMaker)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _dateNamer = dateNamer;
        _fileNamer = fileNamer;
        _substitutionMaker = substitutionMaker;
    }

    public void Execute(CreateOrShowCommand createOrShowCommand)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var path = _fileNamer.FilePathForDate(createOrShowCommand.Date);

        if (!File.Exists(path))
        {
            var templateText = _templateProvider.GetTemplate();

            var outputText = _substitutionMaker.MakeSubstitutions(createOrShowCommand, templateText);
            File.WriteAllText(path, outputText);
        }

        Process.Start(configuration.TextEditorPath, path);
    }
}