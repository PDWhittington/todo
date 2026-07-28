using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowHtmlCommandExecutor(
    IDateListPathResolver dateListPathResolver,
    IOutputWriter outputWriter,
    IHtmlFileLauncher htmlFileLauncher,
    ILogger<ShowHtmlCommandExecutor> logger
) : CommandExecutorBase<ShowHtmlCommand>(outputWriter, logger), IShowHtmlCommandExecutor
{
    public override void Execute(ShowHtmlCommand showHtmlCommand)
    {
        var htmlDocumentInfo = dateListPathResolver.ResolvePathFor(
            showHtmlCommand.Date,
            FileTypeEnum.Html,
            false
        );
        htmlFileLauncher.LaunchFiles(htmlDocumentInfo.Path);
    }
}