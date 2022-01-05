using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.CommandFactories;

public interface ICommitCommandFactory : ICommandFactory<CommitCommand> { }
