using LibGit2Sharp;

namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    Repository Repository { get; }

    IGitInterfaceTools GitInterfaceTools { get; }

    TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : IGitCommand<TResultType>;
}
