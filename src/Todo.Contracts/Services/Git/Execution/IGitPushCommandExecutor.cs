using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Services.Git.Execution;

public interface IGitPushCommandExecutor : IGitCommandExecutor<GitPushCommand, VoidResult>;