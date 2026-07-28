using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitRemoveCommand(params string[] Paths) : IGitCommand<VoidResult>;