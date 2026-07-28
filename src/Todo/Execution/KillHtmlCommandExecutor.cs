using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class KillHtmlCommandExecutor(
    IOutputFolderPathProvider outputFolderPathProvider,
    IFileDeleter fileDeleter,
    IOutputWriter outputWriter,
    ILogger<KillHtmlCommandExecutor> logger
) : CommandExecutorBase<KillHtmlCommand>(outputWriter, logger), IKillHtmlCommandExecutor
{
    public override void Execute(KillHtmlCommand command)
    {
        OutputWriter.WriteLine("Deleting html files in the output and archive folders.");

        fileDeleter.Delete(outputFolderPathProvider.GetRootedOutputFolder(), "*.html");
        fileDeleter.Delete(outputFolderPathProvider.GetRootedArchiveFolder(), "*.html");
    }
}