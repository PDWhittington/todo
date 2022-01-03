using System;
using System.Diagnostics;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.FileNaming;

namespace Todo.Execution;

public class CreateOrShowExecutor : ICreateOrShowExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMarkdownTemplateProvider _templateProvider;
    private readonly IFileNamer _fileNamer;
    private readonly IMarkdownSubstitutionMaker _markdownSubstitutionMaker;
    private readonly ISpecialDateNamer _specialDateNamer;

    public CreateOrShowExecutor(IConfigurationProvider configurationProvider, IMarkdownTemplateProvider templateProvider,
        IFileNamer fileNamer, IMarkdownSubstitutionMaker markdownSubstitutionMaker, ISpecialDateNamer specialDateNamer)
    {
        _configurationProvider = configurationProvider;
        _templateProvider = templateProvider;
        _fileNamer = fileNamer;
        _markdownSubstitutionMaker = markdownSubstitutionMaker;
        _specialDateNamer = specialDateNamer;
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

    private MarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowCommand createOrShowCommand) //DateOnly date)
    {
        string dateText = _configurationProvider.Config.UseNamesForDays && //Check if UseNamesForDays is turned on
                          _specialDateNamer.TryGetSpecialName(createOrShowCommand.Date, out var dateName) // Check if current day is a special day
            ?
            $"{dateName}, {createOrShowCommand.Date.Year}" :
            $"{createOrShowCommand.Date:dddd d}<sup>{GetOrdinal(createOrShowCommand.Date.Day)}</sup> {createOrShowCommand.Date:MMMM}, {createOrShowCommand.Date:yyyy}";

        return MarkdownSubstitutions.Of(dateText);
    }

    private static string GetOrdinal(int num)
    {
        if (num is < 1 or > 31) throw new ArgumentException("Out of range", nameof(num));

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
