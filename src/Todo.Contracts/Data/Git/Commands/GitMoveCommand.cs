using Todo.Contracts.Data.Git.Results;

namespace Todo.Contracts.Data.Git.Commands;

public record GitMoveCommand(string SourcePath, string DestinationPath) : IGitCommand<VoidResult>;