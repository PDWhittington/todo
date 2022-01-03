using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface ISyncExecutor : IExecutor<SyncCommand> { }
