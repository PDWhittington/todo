using System;
using System.IO;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public record GitMoveCommand : IGitCommand<GitVoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string SourcePath { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    public string DestinationPath { get; }

    public GitMoveCommand(string sourcePath, string destinationPath)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
    }

    public GitVoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Moving {SourcePath} to {DestinationPath}");

            gitInterface.GitInterfaceTools.FolderCreator.CreateFromPathIfDoesntExist(DestinationPath);

            File.Move(SourcePath, DestinationPath);
            LibGit2Sharp.Commands.Stage(gitInterface.Repository, SourcePath);
            LibGit2Sharp.Commands.Stage(gitInterface.Repository, DestinationPath);

            return new GitVoidResult(true, null);
        }
        catch (Exception e)
        {
            return new GitVoidResult(false, e);
        }
    }
}
