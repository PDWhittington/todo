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
        try
        {
            foreach (var path in Paths)
            {
                var indexEntries = repo
                    .RetrieveStatus()
                    .Where(x => x.State == FileStatus.NewInIndex)
                    .Select(x => Path.Combine(repo.Info.WorkingDirectory, x.FilePath))
                    .ToArray();

                var pathInIndex = repo.Index.SingleOrDefault(
                    x => path.Equals(Path.Combine(repo.Info.WorkingDirectory, x.Path)));

                if (pathInIndex is null) continue;

                repo.Index.Remove(pathInIndex.Path);
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
