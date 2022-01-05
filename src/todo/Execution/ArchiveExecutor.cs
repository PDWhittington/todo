using System;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class ArchiveExecutor : CommandExecutorBase<ArchiveCommand>, IArchiveExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;
    private readonly IFileNamer _fileNamer;

    public ArchiveExecutor(IConfigurationProvider configurationProvider,
        IGitInterface gitInterface, IFileNamer fileNamer)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
        _fileNamer = fileNamer;
    }

    public override void Execute(ArchiveCommand command)
    {
        if (_configurationProvider.Config.UseGit) Archive(command, GitArchive);
        else Archive(command, FileArchive);
    }

    private void Archive(ArchiveCommand command, Action<FilePathInfo, FilePathInfo> archiveOp)
    {
        var sourcePathInfo = _fileNamer.GetFilePath(command.DateOfFileToArchive, FileTypeEnum.Markdown);
        var destinationPathInfo = _fileNamer.GetArchiveFilePath(command.DateOfFileToArchive, FileTypeEnum.Markdown);

        archiveOp(sourcePathInfo, destinationPathInfo);
    }

    private void GitArchive(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo)
        => _gitInterface.RunGitCommand($"mv {sourcePathInfo.Path} {destinationPathInfo.Path}");

    private static void FileArchive(FilePathInfo sourcePathInfo, FilePathInfo destinationPathInfo)
        => File.Move(sourcePathInfo.Path, destinationPathInfo.Path);
}
