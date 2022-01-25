using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface ICommandExecutorSet
{
    ICommandExecutor? GetExecutorForCommand(CommandBase commandBase);

    // ReSharper disable once UnusedMember.Global
    bool TryExecute<T>(T command) where T : CommandBase;
}
