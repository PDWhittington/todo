using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHtmlCommandExecutor : CommandExecutorBase<ShowHtmlCommand>, IShowHtmlCommandExecutor
{
    private readonly IDateListPathResolver _dateListPathResolver;

    private readonly IHtmlFileLauncher _htmlFileLauncher;

    public ShowHtmlCommandExecutor(IDateListPathResolver dateListPathResolver,
        IOutputWriter outputWriter, IHtmlFileLauncher htmlFileLauncher)
        : base(outputWriter)
    {
        _dateListPathResolver = dateListPathResolver;
        _htmlFileLauncher = htmlFileLauncher;
    }

    public override void Execute(ShowHtmlCommand showHtmlCommand)
    {
        var htmlDocumentInfo = _dateListPathResolver.ResolvePathFor(showHtmlCommand.Date, FileTypeEnum.Html, false);

        _htmlFileLauncher.LaunchFiles(htmlDocumentInfo.Path);
    }
}
