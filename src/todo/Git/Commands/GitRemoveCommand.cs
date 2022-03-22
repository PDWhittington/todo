using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Git.Results;

namespace Todo.Git.Commands;

public class GitRemoveCommand : GitCommandBase<VoidResult>
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string [] Paths { get; }

    public GitRemoveCommand(params string [] paths)
    {
        Paths = paths;
    }

    internal override VoidResult ExecuteCommand(IGitInterface gitInterface)
    {
        bool StatusEntryIsPath(StatusEntry statusEntry, string path)
        {
            var rootedPath = Path.Combine(
                gitInterface.Repository.Info.WorkingDirectory,
                statusEntry.FilePath);

            var fileInfo = new FileInfo(rootedPath);
            return fileInfo.FullName.Equals(path);
        }

        try
        {
            foreach (var path in Paths)
            {
                var pathInIndex = gitInterface.Repository
                    .RetrieveStatus()
                    .SingleOrDefault(x => StatusEntryIsPath(x, path));

                if (pathInIndex == null) continue;

                gitInterface.Repository.Index.Remove(pathInIndex.FilePath);
            }

            LibGit2Sharp.Commands.Remove(gitInterface.Repository, Paths);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
