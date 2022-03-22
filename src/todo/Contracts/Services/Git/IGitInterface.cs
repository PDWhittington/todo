using LibGit2Sharp;
using Todo.Git.Commands;

namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    IRepository Repository { get; }

    IGitInterfaceTools GitInterfaceTools { get; }

    TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : GitCommandBase<TResultType>;
}
