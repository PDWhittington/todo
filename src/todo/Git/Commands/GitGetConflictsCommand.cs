using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Commands;

public class GitGetConflictsCommand : GitCommandBase<ConflictCollection>
{
    internal override ConflictCollection ExecuteCommand(IGitInterface gitInterface)
    {
        gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
            "Retrieving conflicts from git index");

        return gitInterface.Repository.Index.Conflicts;
    }
}
