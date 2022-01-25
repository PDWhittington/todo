using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.DateNaming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Templates;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowDayListCommandExecutor
    : CreateOrShowCommandExecutorBase<CreateOrShowDayListCommand, DayListMarkdownSubstitutions>,
        ICreateOrShowDayListCommandExecutor
{
    private readonly IMarkdownTemplateProvider _templateProvider;
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IDayListMarkdownSubstitutionsMaker _markdownSubstitutionMaker;
    private readonly IDateFormatter _dateFormatter;

    public CreateOrShowDayListCommandExecutor(IMarkdownTemplateProvider templateProvider,
        IDateListPathResolver dateListPathResolver, IDayListMarkdownSubstitutionsMaker markdownSubstitutionMaker,
        IDateFormatter dateFormatter, IFileOpener fileOpener)
        : base (fileOpener)
    {
        _templateProvider = templateProvider;
        _dateListPathResolver = dateListPathResolver;
        _markdownSubstitutionMaker = markdownSubstitutionMaker;
        _dateFormatter = dateFormatter;
    }

    protected override FilePathInfo GetFilePathInfo(CreateOrShowDayListCommand createOrShowCommand)
        => _dateListPathResolver.ResolvePathFor(createOrShowCommand.Date, FileTypeEnum.Markdown, true);

    protected override TodoFile GetTemplate()
        => _templateProvider.GetTemplate(MarkdownTemplateEnum.DayListTemplate);

    protected override DayListMarkdownSubstitutions GetMarkdownSubstitutions(CreateOrShowDayListCommand createOrShowCommand)
    {
        var dateText = _dateFormatter.GetMarkdownHeader(createOrShowCommand.Date);

        return DayListMarkdownSubstitutions.Of(dateText);
    }

    protected override string MakeSubstitutions(DayListMarkdownSubstitutions markdownSubstitutions, string fileContents)
        => _markdownSubstitutionMaker.MakeSubstitutions(markdownSubstitutions, fileContents);
}
