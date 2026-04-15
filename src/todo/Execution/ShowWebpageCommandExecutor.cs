using System.Diagnostics.CodeAnalysis;
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
    IOutputWriter outputWriter)
    : CommandExecutorBase<ShowWebpageCommand>(outputWriter), IShowWebpageCommandExecutor
{
    public override void Execute(ShowWebpageCommand command)
    {
        htmlFileLauncher.LaunchFiles(constantsProvider.ProjectWebsite);
    }
}
