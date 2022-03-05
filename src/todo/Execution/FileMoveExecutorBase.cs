using System;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;
using Todo.Git.Results;

namespace Todo.Execution;

public abstract class FileMoveExecutorBase<T> : CommandExecutorBase<T> where T : FileMoveCommandBase
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;

    protected FileMoveExecutorBase(IConfigurationProvider configurationProvider,
        IGitInterface gitInterface, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
    }

    public override void Execute(T command)
    {
        if (_configurationProvider.Config.UseGit) MoveFile(command, MoveFileInGit);
        else MoveFile(command, MoveFileWithoutGit);
    }

    protected abstract FilePathInfo GetSourcePath(T command);

    protected abstract FilePathInfo GetDestinationPath(T command);

    private void MoveFile(T command, Action<FilePathInfo, FilePathInfo> moveOperation)
    {
        var sourcePathInfo = GetSourcePath(command);
        var destinationPathInfo = GetDestinationPath(command);

        moveOperation(sourcePathInfo, destinationPathInfo);
    }

    private void MoveFileInGit(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo)
        => _gitInterface.RunGitCommand<GitMoveCommand, VoidResult>(
            new GitMoveCommand(sourcePathInfo.Path, destinationPathInfo.Path));

    private void MoveFileWithoutGit(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo)
    {
        OutputWriter.WriteLine($"Moving {sourcePathInfo.Path} to {destinationPathInfo.Path}");
        File.Move(sourcePathInfo.Path, destinationPathInfo.Path);
    }
}
