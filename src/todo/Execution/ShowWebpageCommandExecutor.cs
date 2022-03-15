using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowWebpageCommandExecutor : CommandExecutorBase<ShowWebpageCommand>, IShowWebpageCommandExecutor
{
    private readonly IConstantsProvider _constantsProvider;

    private readonly IHtmlFileLauncher _htmlFileLauncher;

    public ShowWebpageCommandExecutor(IHtmlFileLauncher htmlFileLauncher,
        IConstantsProvider constantsProvider, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _constantsProvider = constantsProvider;
        _htmlFileLauncher = htmlFileLauncher;
    }

    public override void Execute(ShowWebpageCommand command)
    {
        _htmlFileLauncher.LaunchFiles(_constantsProvider.ProjectWebsite);
    }
}
