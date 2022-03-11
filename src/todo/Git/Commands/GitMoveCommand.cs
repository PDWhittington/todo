using System;
using System.IO;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitMoveCommand : GitCommandBase<VoidResult>
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

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        try
        {
            outputWriter?.WriteLine($"Moving {SourcePath} to {DestinationPath}");

            File.Move(SourcePath, DestinationPath);
            LibGit2Sharp.Commands.Stage(repo, SourcePath);
            LibGit2Sharp.Commands.Stage(repo, DestinationPath);

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
