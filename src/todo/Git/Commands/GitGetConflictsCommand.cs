using LibGit2Sharp;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Commands;

public class GitGetConflictsCommand : GitCommandBase<ConflictCollection>
{
    internal override ConflictCollection ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        outputWriter?.WriteLine("Retrieving conflicts from git index");
        return repo.Index.Conflicts;
    }
}
