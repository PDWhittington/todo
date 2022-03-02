using Todo.Git.Commands;

namespace Todo.Contracts.Services.Git;

public interface IGitInterface
{
    TResultType RunGitCommand<TCommandType, TResultType>(TCommandType command)
        where TCommandType : GitCommandBase<TResultType>;
}
