using LibGit2Sharp;
using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

public record GitGetConflictsCommand : IGitCommand<ConflictCollection>
{
    public ConflictCollection ExecuteCommand(IGitInterface gitInterface)
    {
        gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
            "Retrieving conflicts from git index");

        return gitInterface.Repository.Index.Conflicts;
    }
}
