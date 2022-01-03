using Todo.Contracts.Data.Commands;
using Todo.Execution;

namespace Todo.Contracts.Services.Execution;

public interface IPushExecutor : IExecutor<PushCommand> { }