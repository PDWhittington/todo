using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

public abstract class FileMoveExecutorBase<T>(
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    IOutputWriter outputWriter,
    IFolderCreator folderCreator,
    ILogger<FileMoveExecutorBase<T>> logger
) : CommandExecutorBase<T>(outputWriter, logger)
    where T : FileMoveCommandBase
{
    public override void Execute(T command)
    {
        if (configurationProvider.ConfigInfo.Configuration.UseGit)
            MoveFile(command, MoveFileInGit);
        else
            MoveFile(command, MoveFileWithoutGit);
    }

    protected abstract FilePathInfo GetSourcePath(T command);

    protected abstract FilePathInfo GetDestinationPath(T command);

    private void MoveFile(T command, Action<FilePathInfo, FilePathInfo> moveOperation)
    {
        var sourcePathInfo = GetSourcePath(command);

        if (!sourcePathInfo.Exists())
            throw new Exception($"Source path not found: {sourcePathInfo.Path}");

        var destinationPathInfo = GetDestinationPath(command);

        moveOperation(sourcePathInfo, destinationPathInfo);
    }

    private void MoveFileInGit(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo) =>
        gitInterface.RunGitCommand<GitMoveCommand, VoidResult>(
            new GitMoveCommand(sourcePathInfo.Path, destinationPathInfo.Path)
        );

    private void MoveFileWithoutGit(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo)
    {
        OutputWriter.WriteLine($"Moving {sourcePathInfo.Path} to {destinationPathInfo.Path}");

        folderCreator.CreateFromPathIfDoesntExist(destinationPathInfo.Path);
        File.Move(sourcePathInfo.Path, destinationPathInfo.Path);
    }
}
