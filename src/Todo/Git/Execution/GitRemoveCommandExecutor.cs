using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Git.Execution;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public class GitRemoveCommandExecutor(IOutputWriter outputWriter, ILogger<GitRemoveCommandExecutor> logger)
    : GitCommandExecutorBase<GitRemoveCommand, VoidResult>(outputWriter, logger),
        IGitRemoveCommandExecutor
{
    private static string ToRepoRelativePath(string workingDirectory, string absolutePath)
    {
        if (string.IsNullOrEmpty(workingDirectory))
            return absolutePath;

        var rel = Path.GetRelativePath(workingDirectory, absolutePath);

        if (Path.DirectorySeparatorChar != '/')
            rel = rel.Replace(Path.DirectorySeparatorChar, '/');

        return rel;
    }

    private static string Pluralise(int number, string singular, string plural) =>
        number == 1 ? $"{number} {singular}" : $"{number} {plural}";

    private static string BuildCheckpointMessage(string[] paths)
    {
        if (paths.Length == 0)
            throw new ArgumentException("Should not be committing a set of zero files.");

        var sb = new StringBuilder().AppendLine(
            $"Checkpoint before removing {Pluralise(paths.Length, "file", "files")}:-"
        );

        foreach (var path in paths)
        {
            sb.AppendLine($"\t{path}");
        }

        return sb.ToString();
    }

    private static bool IsChangedFromHead(
        HashSet<string> dirtyPaths,
        string workDir,
        string relPath,
        string absPath
    )
    {
        if (string.IsNullOrEmpty(workDir))
            return dirtyPaths.Contains(relPath);

        var fullWork =
            Path.GetFullPath(workDir)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        var fullPath = Path.GetFullPath(absPath);

        return fullPath.StartsWith(fullWork, StringComparison.OrdinalIgnoreCase)
            &&
            // Path is outside the repository working directory; cannot checkpoint via this repo.
            // Fast hash-based lookup (amortized O(1) average case) against the single RetrieveStatus result.
            dirtyPaths.Contains(relPath);
    }

    public override VoidResult RunGitCommand(IGitInterface gitInterface, GitRemoveCommand command)
    {
        try
        {
            var repo = gitInterface.Repository;
            var workDir = repo.Info.WorkingDirectory ?? string.Empty;

            // One status call for the whole repo (cheap for a typical todo working tree).
            // We then do fast in-memory membership tests for the specific paths we care about.
            var status = repo.RetrieveStatus(new StatusOptions { IncludeUnaltered = false });

            var dirtyRelPaths = new HashSet<string>(
                status.Select(e => e.FilePath),
                StringComparer.Ordinal
            );

            //Find which files of the set we are to remove have changed -- whether indexed or not.
            var pathsNeedingStaging = command
                .Paths.Where(path =>
                {
                    var relPath = ToRepoRelativePath(workDir, path);
                    return IsChangedFromHead(dirtyRelPaths, workDir, relPath, path);
                })
                .ToArray();

            if (pathsNeedingStaging.Length != 0)
            {
                gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
                    "Staging changed files before deletion:"
                );

                foreach (var path in pathsNeedingStaging)
                {
                    gitInterface.GitInterfaceTools.OutputWriter.WriteLine($"Staging {path}");
                }

                LibGit2Sharp.Commands.Stage(repo, pathsNeedingStaging);

                var commitMessage = BuildCheckpointMessage(pathsNeedingStaging);
                var commitResult = gitInterface.RunGitCommand<GitCommitCommand, CommitResult>(
                    new GitCommitCommand(commitMessage)
                );

                if (commitResult is { Success: false, Exception: not EmptyCommitException })
                {
                    return new VoidResult(false, commitResult.Exception);
                }
            }

            // Remove (delete) all specified files from the filesystem.
            // This is not staged or committed; the user can commit the deletions separately if desired.
            foreach (var path in command.Paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}

