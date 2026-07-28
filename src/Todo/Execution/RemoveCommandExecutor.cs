using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class RemoveCommandExecutor(
    IOutputWriter outputWriter,
    IConfigurationProvider configurationProvider,
    IDateListPathResolver dateListPathResolver,
    IGitInterface gitInterface,
    ILogger<RemoveCommandExecutor> logger)
    : CommandExecutorBase<RemoveCommand>(outputWriter, logger), IRemoveCommandExecutor
{
    public override void Execute(RemoveCommand command)
    {
        Logger.LogInformation("In {GetType}.{MethodName}: Received date {CommandDate:yyyy-MM-dd}.", 
            GetType(), nameof(Execute), command.Date);

        if (!dateListPathResolver.TryResolvePathFor(command.Date, FileTypeEnum.MarkdownDayList, out var pathForFile))
        {
            Logger.LogInformation("In {GetType}.{MethodName}: Could not find file for date {CommandDate:yyyy-MM-dd}.",
                GetType(), nameof(Execute), command.Date);

            OutputWriter.WriteLine($"Could not find file for {command.Date}.");
            return;
        }

        Logger.LogInformation("In {GetType}.{MethodName}: Found path to delete: {pathForFile}.",
            GetType(), nameof(Execute), pathForFile!);

        if (configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            Logger.LogInformation("In {GetType}.{MethodName}: Git is enabled.",
                GetType(), nameof(Execute));

            var gitRemoveCommand = new GitRemoveCommand(pathForFile!.Path);

            Logger.LogInformation("In {GetType}.{MethodName}: GitRemoveCommand created. Running GitRemoveCommand...",
                GetType(), nameof(Execute));

            var gitResult = gitInterface.RunGitCommand<GitRemoveCommand, VoidResult>(gitRemoveCommand);

            Logger.LogInformation("In {GetType}.{MethodName}: GitRemoveCommand run. Result: {Result}",
                GetType(), nameof(Execute), gitResult.Success ? "Success" : "Failure");

            if (!gitResult.Success) throw gitResult.Exception ?? new Exception("Some exception in git rm command");
        }
        else
        {
            Logger.LogInformation("In {GetType}.{MethodName}: Git is not enabled. Deleting file from file system.",
                GetType(), nameof(Execute));

            File.Delete(pathForFile!.Path);
        }
    }
}