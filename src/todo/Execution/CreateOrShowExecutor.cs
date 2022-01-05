using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMarkdownTemplateProvider _templateProvider;
    private readonly IContentFilePathResolver _fileResolver;
    private readonly IMarkdownSubstitutionsMaker _markdownSubstitutionMaker;
    private readonly IDateFormatter _dateFormatter;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, IMarkdownTemplateProvider templateProvider,
        IContentFilePathResolver fileResolver, IMarkdownSubstitutionsMaker markdownSubstitutionMaker,
        IDateFormatter dateFormatter)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _fileResolver = fileResolver;
        _markdownSubstitutionMaker = markdownSubstitutionMaker;
        _dateFormatter = dateFormatter;
    }

    public void Execute(CreateOrShowCommand createOrShowCommand)
    {
        var pathInfo = _fileResolver.GetPathFor(createOrShowCommand.Date, FileTypeEnum.Markdown, true);

        if (!File.Exists(pathInfo.Path))
        {
            var templateFile = _templateProvider.GetTemplate();

            var markdownSubstitutions = GetMarkdownSubstitutions(createOrShowCommand);

            var outputText = _markdownSubstitutionMaker.MakeSubstitutions(markdownSubstitutions, templateFile.FileContents);
            File.WriteAllText(pathInfo.Path, outputText);
        }

        Process.Start(_configurationProvider.Config.TextEditorPath, pathInfo.Path);
    }

    private MarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowCommand createOrShowCommand)
    {
        var dateText = _dateFormatter.GetMarkdownHeader(createOrShowCommand.Date);

        return MarkdownSubstitutions.Of(dateText);
    }


}
