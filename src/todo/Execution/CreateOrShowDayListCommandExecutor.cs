using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowDayListCommandExecutor
    : CreateOrShowCommandExecutorBase<CreateOrShowDayListCommand, DayListMarkdownSubstitutions>,
        ICreateOrShowDayListCommandExecutor
{
    private readonly IDayListMarkdownTemplateProvider _dayListMarkdownTemplateProvider;
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IDayListMarkdownSubstitutionsMaker _markdownSubstitutionMaker;
    private readonly IDateFormatter _dateFormatter;

    public CreateOrShowDayListCommandExecutor(IDayListMarkdownTemplateProvider dayListMarkdownTemplateProvider,
        IDateListPathResolver dateListPathResolver, IDayListMarkdownSubstitutionsMaker markdownSubstitutionMaker,
        IDateFormatter dateFormatter, IConfigurationProvider configurationProvider,
        IGitInterface gitInterface, IFileOpener fileOpener, IOutputWriter outputWriter)
        : base (configurationProvider, gitInterface, fileOpener, outputWriter)
    {
        _dayListMarkdownTemplateProvider = dayListMarkdownTemplateProvider;
        _dateListPathResolver = dateListPathResolver;
        _markdownSubstitutionMaker = markdownSubstitutionMaker;
        _dateFormatter = dateFormatter;
    }

    protected override FilePathInfo GetFilePathInfo(CreateOrShowDayListCommand createOrShowCommand)
        => _dateListPathResolver.ResolvePathFor(createOrShowCommand.Date, FileTypeEnum.Markdown, true);

    protected override TodoFile GetTemplate() => _dayListMarkdownTemplateProvider.GetTemplate();

    protected override DayListMarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowDayListCommand createOrShowCommand)
    {
        var dateText = _dateFormatter.GetMarkdownHeader(createOrShowCommand.Date);

        return DayListMarkdownSubstitutions.Of(dateText);
    }

    protected override string MakeSubstitutions(DayListMarkdownSubstitutions markdownSubstitutions, string fileContents)
        => _markdownSubstitutionMaker.MakeSubstitutions(markdownSubstitutions, fileContents);
}
