using System;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git.Execution;

namespace Todo.Git.Execution;

public class GitCommandExecutorResolver(
    IGitAddCommandExecutor gitAddCommandExecutor,
    IGitCommitCommandExecutor gitCommitCommandExecutor,
    IGitGetConflictsCommandExecutor gitGetConflictsCommandExecutor,
    IGitGetRepoInfoCommandExecutor gitGetRepoInfoCommandExecutor,
    IGitMoveCommandExecutor gitMoveCommandExecutor,
    IGitPushCommandExecutor gitPushCommandExecutor,
    IGitRemoveCommandExecutor gitRemoveCommandExecutor,
    IGitResetCommandExecutor gitResetCommandExecutor)
    : IGitCommandExecutorResolver
{

    public IGitCommandExecutor<TCommandType, TGitResult> Resolve<TCommandType, TGitResult>(
        TCommandType command) 
        where TCommandType : IGitCommand<TGitResult> 
        where TGitResult : GitResultBase
    {
        return command switch
        {
            GitAddCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitAddCommandExecutor,
            GitCommitCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitCommitCommandExecutor,
            GitGetConflictsCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitGetConflictsCommandExecutor,
            GitGetRepoInfoCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitGetRepoInfoCommandExecutor,
            GitMoveCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitMoveCommandExecutor,
            GitPushCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitPushCommandExecutor,
            GitRemoveCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitRemoveCommandExecutor,
            GitResetCommand => (IGitCommandExecutor<TCommandType, TGitResult>)gitResetCommandExecutor,
            _ => throw new Exception()
        };
    }
}