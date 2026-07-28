using LibGit2Sharp;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    Repository Repository { get; }

    TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : IGitCommand<TResultType>
        where TResultType : GitResultBase;
}
