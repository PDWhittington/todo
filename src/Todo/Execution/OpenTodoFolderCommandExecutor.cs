using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class OpenTodoFolderCommandExecutor(
    IOutputWriter outputWriter,
    IOutputFolderPathProvider outputFolderPathProvider,
    IFileExplorerLauncher fileExplorerLauncher,
    ILogger<OpenTodoFolderCommandExecutor> logger)
    : CommandExecutorBase<OpenTodoFolderCommand>(outputWriter, logger), IOpenTodoFolderCommandExecutor
{
    public override void Execute(OpenTodoFolderCommand command)
    {
        var todoFolder = outputFolderPathProvider.GetRootedOutputFolder();
        fileExplorerLauncher.LaunchFiles(todoFolder);
    }
}