using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowConflictsCommandExecutor(
    IOutputWriter outputWriter,
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    ITextFileLauncher fileOpener,
    ILogger<ShowConflictsCommandExecutor> logger
) : CommandExecutorBase<ShowConflictsCommand>(outputWriter, logger), IShowConflictsCommandExecutor
{
    public override void Execute(ShowConflictsCommand command)
    {
        if (!configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            OutputWriter.WriteLine("This workspace is configured not to use git.");
            return;
        }

        var conflictsResults = gitInterface
            .RunGitCommand<GitGetConflictsCommand, ConflictsResult>(GitGetConflictsCommand.Instance);

        if (!conflictsResults.Success)
            throw new Exception(); //Handle this more gracefully

        var conflictCollection = conflictsResults.ConflictCollection!;

        if (!conflictCollection.Any())
        {
            OutputWriter.WriteLine("There are no conflicts in the current git");
            return;
        }

        var paths = conflictCollection
            .SelectMany(conflict => new[] { conflict.Ours.Path, conflict.Theirs.Path })
            .Distinct()
            .Where(File.Exists)
            .ToArray();

        fileOpener.LaunchFiles(paths);
    }
}