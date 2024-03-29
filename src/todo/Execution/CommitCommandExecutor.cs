﻿using System;
using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;
using Todo.Git.Results;
using CommitCommand = Todo.Contracts.Data.Commands.CommitCommand;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class CommitCommandExecutor : CommandExecutorBase<CommitCommand>, ICommitCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    public CommitCommandExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface,
        IOutputFolderPathProvider outputFolderPathProvider, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
        _outputFolderPathProvider = outputFolderPathProvider;
    }

    public override void Execute(CommitCommand commitCommand)
    {
        if (!_configurationProvider.ConfigInfo.Configuration.UseGit)
            throw new Exception("Syncing does not make sense when UseGit is set to false in the settings file.");

        var commitMessage = commitCommand.CommitMessage ?? $"Synced as at {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        OutputWriter.WriteLine("Committing todo files.");

        _gitInterface.RunGitCommand<GitResetCommand, VoidResult>(new GitResetCommand());
        _gitInterface.RunGitCommand<GitAddCommand, VoidResult>(
            new GitAddCommand(_outputFolderPathProvider.GetRootedOutputFolder()));

        //Archive may not be nested within the OutputFolder
        _gitInterface.RunGitCommand<GitAddCommand, VoidResult>(
            new GitAddCommand(_outputFolderPathProvider.GetRootedArchiveFolder()));

        var commitResult = _gitInterface.RunGitCommand<GitCommitCommand, CommitResult>(
            new GitCommitCommand(commitMessage));

        if (commitResult.Exception is EmptyCommitException)
        {
            OutputWriter.WriteLine("No commit written -- was empty");
        }
    }
}
