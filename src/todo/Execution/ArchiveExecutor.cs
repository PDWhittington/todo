using System;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class ArchiveExecutor : IArchiveExecutor
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

    public void Execute(ArchiveCommand command)
    {
        var configuration = _configurationProvider.GetConfiguration();
        
        if (configuration.UseGit) Archive(command, GitArchive);
        else Archive(command, FileArchive);
    }

    private void Archive(ArchiveCommand command, Action<string, string> archiveOp)
    {
        var sourcePath = _fileNamer.MarkdownFilePathForDate(command.DateOfFileToArchive);
        var destinationPath = _fileNamer.MarkdownArchiveFilePathForDate(command.DateOfFileToArchive);

        archiveOp(sourcePath, destinationPath);
    }
    
    private void GitArchive(string sourcePath, string destinationPath) => _gitInterface.RunGitCommand($"mv {sourcePath} {destinationPath}");

    private void FileArchive(string sourcePath, string destinationPath) => File.Move(sourcePath, destinationPath);
}