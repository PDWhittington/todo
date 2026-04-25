using System;
using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface ICommandExecutor<in T> : ICommandExecutor
    where T : CommandBase
{
    void Execute(T command);
}

public interface ICommandExecutor
{
    Type CommandType { get; }

    void ExecuteCommandBase(CommandBase command);
}
