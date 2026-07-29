using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitMoveCommandExecutor(IFolderCreator folderCreator, IFileMover fileMover,
    IOutputWriter outputWriter, ILogger<GitMoveCommandExecutor> logger)
    : GitCommandExecutorBase<GitMoveCommand, VoidResult>(outputWriter, logger),
        IGitMoveCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, 
        GitMoveCommand gitMoveCommand)
    {
        Logger.LogInformation(
            "In {GetType}.{MethodName}: Received {TypeName} (SourcePath: {sourcePath}, DestinationPath: {destinationPath}).",
            GetType(),
            nameof(RunGitCommand),
            gitMoveCommand.GetType().Name,
            gitMoveCommand.SourcePath,
            gitMoveCommand.DestinationPath);

        try
        {
            OutputWriter.WriteLine(
            $"Moving {gitMoveCommand.SourcePath} to {gitMoveCommand.DestinationPath}");

            folderCreator.CreateFromPathIfDoesntExist(
                gitMoveCommand.DestinationPath);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Physically moving file (SourcePath: {sourcePath}, DestinationPath: {destinationPath})...",
                GetType(),
                nameof(RunGitCommand),
                gitMoveCommand.SourcePath,
                gitMoveCommand.DestinationPath);

            fileMover.Move(gitMoveCommand.SourcePath, gitMoveCommand.DestinationPath);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Attempting to stage source path: {sourcePath}",
                GetType(),
                nameof(RunGitCommand),
                gitMoveCommand.SourcePath);

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitMoveCommand.SourcePath);

            Logger.LogInformation(
                "In {GetType}.{MethodName}: Attempting to stage destination path: {destinationPath}",
                GetType(),
                nameof(RunGitCommand),
                gitMoveCommand.DestinationPath);

            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitMoveCommand.DestinationPath);

            return new VoidResult(true);
        }
        catch (Exception e)
        {
            Logger.LogError(
                "In {GetType}.{MethodName}: Move failed. Exception message: {exceptionMessage}...",
                GetType(),
                nameof(RunGitCommand),
                e.Message);

            return new VoidResult(false, e);
        }
    }
}
