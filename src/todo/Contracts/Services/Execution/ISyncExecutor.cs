using Todo.Contracts.Data.Commands;

namespace Todo.Execution;

public interface ISyncExecutor : IExecutor<SyncCommand>
{
    
}