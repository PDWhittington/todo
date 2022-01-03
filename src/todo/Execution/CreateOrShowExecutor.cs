using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.FileSystem;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMarkdownTemplateProvider _templateProvider;
    private readonly IFileNamer _fileNamer;
    private readonly IMarkdownSubstitutionsMaker _markdownSubstitutionMaker;
    private readonly ISpecialDateNamer _specialDateNamer;
    private readonly IDateFormatter _dateFormatter;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, IMarkdownTemplateProvider templateProvider,
        IFileNamer fileNamer, IMarkdownSubstitutionsMaker markdownSubstitutionMaker, ISpecialDateNamer specialDateNamer,
        IDateFormatter dateFormatter)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _fileNamer = fileNamer;
        _markdownSubstitutionMaker = markdownSubstitutionMaker;
        _specialDateNamer = specialDateNamer;
        _dateFormatter = dateFormatter;
    }

    public void Execute(CreateOrShowCommand createOrShowCommand)
    {
        var path = _fileNamer.GetFilePath(createOrShowCommand.Date, FileTypeEnum.Markdown);

        if (!File.Exists(path))
        {
            var templateText = _templateProvider.GetTemplate();

            var markdownSubstitutions = GetMarkdownSubstitutions(createOrShowCommand);

            var outputText = _markdownSubstitutionMaker.MakeSubstitutions(markdownSubstitutions, templateText);
            File.WriteAllText(path, outputText);
        }

        Process.Start(_configurationProvider.Config.TextEditorPath, path);
    }

    private MarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowCommand createOrShowCommand)
    {
        string dateText = _dateFormatter.GetMarkdownHeader(createOrShowCommand.Date);

        return MarkdownSubstitutions.Of(dateText);
    }


}
