using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Git.Ancillary;

namespace Todo.Contracts.Data.Git.Commands;

public record GitPushCommand(IBranchLocator BranchLocator) : IGitCommand<VoidResult>;
