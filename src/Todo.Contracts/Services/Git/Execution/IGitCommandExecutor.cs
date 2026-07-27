using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Services.Git.Execution;

public interface IGitCommandExecutor<in TCommandType, out TResultType>
    where TCommandType : IGitCommand<TResultType>
    where TResultType : GitResultBase
{
    TResultType RunGitCommand(IGitInterface gitInterface, TCommandType command);
}

