using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandExecutor(
    ISettingsPathProvider settingsPathProvider,
    ITextFileLauncher fileOpener,
    IOutputWriter outputWriter,
    ILogger<ShowSettingsCommandExecutor> logger
) : CommandExecutorBase<ShowSettingsCommand>(outputWriter, logger), IShowSettingsCommandExecutor
{
    public override void Execute(ShowSettingsCommand _)
    {
        var path = settingsPathProvider.GetSettingsPathInHierarchy().Path;

        OutputWriter.WriteLine($"Opening {path}");
        fileOpener.LaunchFiles(path);
    }
}
