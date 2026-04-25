using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface IUnarchiveCommandExector : ICommandExecutor<UnarchiveCommand>;