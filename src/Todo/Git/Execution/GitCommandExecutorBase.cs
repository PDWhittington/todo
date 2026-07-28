using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.UI;

namespace Todo.Git.Execution;

public abstract class GitCommandExecutorBase<TCommandType, TResultType>(
    IOutputWriter outputWriter,
    ILogger<GitCommandExecutorBase<TCommandType, TResultType>> logger)
    where TCommandType : IGitCommand<TResultType>
    where TResultType : GitResultBase
{
    protected IOutputWriter OutputWriter { get; } = outputWriter;
    protected ILogger<GitCommandExecutorBase<TCommandType, TResultType>> Logger { get; } = logger;

    // ReSharper disable once UnusedMember.Global
    public abstract TResultType RunGitCommand(IGitInterface gitInterface, TCommandType gitCommand);
}