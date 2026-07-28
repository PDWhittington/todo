using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface IOpenTodoFolderCommandExecutor : ICommandExecutor<OpenTodoFolderCommand>;