using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitMoveCommandExecutor(IOutputWriter outputWriter, ILogger<GitMoveCommandExecutor> logger)
    : GitCommandExecutorBase<GitMoveCommand, VoidResult>(outputWriter, logger),
        IGitMoveCommandExecutor
{
    public override VoidResult RunGitCommand(IGitInterface gitInterface, 
        GitMoveCommand gitMoveCommand)
    {
        try
        {
            gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                $"Moving {gitMoveCommand.SourcePath} to {gitMoveCommand.DestinationPath}"
            );

            gitInterface.GitInterfaceTools.FolderCreator.CreateFromPathIfDoesntExist(
                gitMoveCommand.DestinationPath
            );

            File.Move(gitMoveCommand.SourcePath, gitMoveCommand.DestinationPath);
            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitMoveCommand.SourcePath);
            LibGit2Sharp.Commands.Stage(gitInterface.Repository, gitMoveCommand.DestinationPath);

            return new VoidResult(true);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

