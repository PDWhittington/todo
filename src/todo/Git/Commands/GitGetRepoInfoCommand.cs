using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Commands;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GitGetRepoInfoCommand : GitCommandBase<RepositoryInformation>
{
    internal override RepositoryInformation ExecuteCommand(IRepository repo, IOutputWriter? outputWriter)
    {
        outputWriter?.WriteLine("Retrieving repo information . . . ");
        return repo.Info;
    }
}
