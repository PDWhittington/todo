using LibGit2Sharp;
using Todo.Contracts.Services.Reporting;

namespace Todo.Git.Commands;

public class GitGetRepoInfoCommand : GitCommandBase<RepositoryInformation>
{
    internal override RepositoryInformation ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        outputWriter?.WriteLine("Retrieving repo information . . . ");
        return repo.Info;
    }
}
