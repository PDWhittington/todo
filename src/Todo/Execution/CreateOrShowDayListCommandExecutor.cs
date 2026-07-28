using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Dates.Naming;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.Templates;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CreateOrShowDayListCommandExecutor(
    IDayListMarkdownTemplateProvider dayListMarkdownTemplateProvider,
    IDateListPathResolver dateListPathResolver,
    IDayListMarkdownSubstitutionsMaker markdownSubstitutionMaker,
    IDateFormatter dateFormatter,
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    ITextFileLauncher fileOpener,
    IOutputWriter outputWriter,
    IFolderCreator folderCreator,
    ILogger<CreateOrShowDayListCommandExecutor> logger
)
    : CreateOrShowCommandExecutorBase<CreateOrShowDayListCommand, DayListMarkdownSubstitutions>(
        configurationProvider,
        gitInterface,
        fileOpener,
        outputWriter,
        folderCreator,
        logger
    ),
        ICreateOrShowDayListCommandExecutor
{
    protected override FilePathInfo GetFilePathInfo(
        CreateOrShowDayListCommand createOrShowCommand
    ) =>
        dateListPathResolver.ResolvePathFor(
            createOrShowCommand.Date,
            FileTypeEnum.MarkdownDayList,
            true
        );

    protected override TodoFile GetTemplate() => dayListMarkdownTemplateProvider.GetTemplate();

    protected override DayListMarkdownSubstitutions GetMarkdownSubstitutions(
        CreateOrShowDayListCommand createOrShowCommand
    )
    {
        var dateText = dateFormatter.GetMarkdownHeader(createOrShowCommand.Date);

        return DayListMarkdownSubstitutions.Of(dateText);
    }

    protected override void MakeSubstitutions(
        DayListMarkdownSubstitutions markdownSubstitutions,
        UnmanagedByteArray fileContents,
        Stream stream
    ) =>
        markdownSubstitutionMaker.WriteSubstitutionsToStream(
            fileContents,
            markdownSubstitutions,
            stream
        );
}