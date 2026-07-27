using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;
using Todo.UI;

namespace Todo.Git.Execution;

public abstract class GitCommandExecutorBase<TCommandType, TResultType>(IOutputWriter outputWriter)
    where TCommandType : IGitCommand<TResultType>
    where TResultType : GitResultBase
{
    protected IOutputWriter OutputWriter { get; } = outputWriter;

    public abstract TResultType RunGitCommand(IGitInterface gitInterface, TCommandType command);
}

