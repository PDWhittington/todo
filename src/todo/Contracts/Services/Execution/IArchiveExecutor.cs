using Todo.Contracts.Data.Commands;

namespace Todo.Execution;

public interface IArchiveExecutor : IExecutor<ArchiveCommand> { }