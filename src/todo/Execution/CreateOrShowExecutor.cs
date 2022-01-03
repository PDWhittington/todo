using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Template;
using Todo.FileNaming;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ITemplateProvider _templateProvider;
    private readonly IFileNamer _fileNamer;
    private readonly ISubstitutionMaker _substitutionMaker;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, ITemplateProvider templateProvider, 
        IFileNamer fileNamer, ISubstitutionMaker substitutionMaker)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _fileNamer = fileNamer;
        _substitutionMaker = substitutionMaker;
    }

    public void Execute(CreateOrShowCommand createOrShowCommand)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var path = _fileNamer.GetFilePath(createOrShowCommand.Date, FileTypeEnum.Markdown);

        if (!File.Exists(path))
        {
            var templateText = _templateProvider.GetTemplate();

            var outputText = _substitutionMaker.MakeSubstitutions(createOrShowCommand, templateText);
            File.WriteAllText(path, outputText);
        }

        Process.Start(configuration.TextEditorPath, path);
    }
}