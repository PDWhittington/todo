using Todo.Contracts.Data.Commands;

namespace Todo.Execution;

public interface IExecutor<T> where T : CommandBase
{
    void Execute(T command);
}