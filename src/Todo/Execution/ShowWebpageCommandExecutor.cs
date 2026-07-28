using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowWebpageCommandExecutor(
    IHtmlFileLauncher htmlFileLauncher,
    IConstantsProvider constantsProvider,
    IOutputWriter outputWriter,
    ILogger<ShowWebpageCommandExecutor> logger)
    : CommandExecutorBase<ShowWebpageCommand>(outputWriter, logger), IShowWebpageCommandExecutor
{
    public override void Execute(ShowWebpageCommand command)
    {
        htmlFileLauncher.LaunchFiles(constantsProvider.ProjectWebsite);
    }
}