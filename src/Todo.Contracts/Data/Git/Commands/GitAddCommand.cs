using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitAddCommand(string Path) : IGitCommand<VoidResult>;