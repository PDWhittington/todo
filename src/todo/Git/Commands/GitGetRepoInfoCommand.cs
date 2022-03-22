using System.Diagnostics.CodeAnalysis;
using LibGit2Sharp;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Commands;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GitGetRepoInfoCommand : GitCommandBase<RepositoryInformation>
{
    internal override RepositoryInformation ExecuteCommand(IGitInterface gitInterface)
    {
        gitInterface.GitInterfaceTools.OutputWriter.WriteLine(
            "Retrieving repo information . . . ");

        return gitInterface.Repository.Info;
    }
}
