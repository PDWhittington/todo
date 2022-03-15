using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;
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

    internal override VoidResult ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        bool StatusEntryIsPath(StatusEntry statusEntry, string path)
        {
            var rootedPath = Path.Combine(repo.Info.WorkingDirectory, statusEntry.FilePath);
            var fileInfo = new FileInfo(rootedPath);
            return fileInfo.FullName.Equals(path);
        }

        try
        {
            foreach (var path in Paths)
            {
                var pathInIndex = repo
                    .RetrieveStatus()
                    .SingleOrDefault(x => StatusEntryIsPath(x, path));

                if (pathInIndex == null) continue;

                repo.Index.Remove(pathInIndex.FilePath);
            }

            LibGit2Sharp.Commands.Remove(repo, Paths);
            return new VoidResult(true, null);
        }
        catch (Exception e)
        {
            return new VoidResult(false, e);
        }
    }
}
