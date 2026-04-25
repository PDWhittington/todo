using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;

namespace Todo.Git.Commands;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public record GitGetRepoInfoCommand : IGitCommand<RepositoryInformation>
{
    public RepositoryInformation ExecuteCommand(IGitInterface gitInterface)
    {
        gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
            "Retrieving repo information . . . ");

        return gitInterface.Repository.Info;
    }
}
