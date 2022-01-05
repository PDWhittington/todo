using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface ICommandExecutorSet
{
    IExecutor? GetExecutorForCommand(CommandBase commandBase);

    bool TryExecute<T>(T command) where T : CommandBase;
}
