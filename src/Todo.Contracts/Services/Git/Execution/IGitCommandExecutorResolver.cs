using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Services.Git.Execution;

public interface IGitCommandExecutorResolver
{
    IGitCommandExecutor<TCommandType, TGitResult> Resolve<TCommandType, TGitResult>(
        TCommandType command
    )
        where TCommandType : IGitCommand<TGitResult>
        where TGitResult : GitResultBase;
}