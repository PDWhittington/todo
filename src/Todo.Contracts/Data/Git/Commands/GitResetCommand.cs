using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitResetCommand(bool Hard = false) : IGitCommand<VoidResult>;

