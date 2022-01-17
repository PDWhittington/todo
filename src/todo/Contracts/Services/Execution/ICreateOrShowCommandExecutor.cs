using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface ICreateOrShowCommandExecutor : ICommandExecutor<CreateOrShowCommand> { }
