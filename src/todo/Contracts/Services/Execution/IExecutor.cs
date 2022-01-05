using System;
using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.Execution;

public interface IExecutor<in T> : IExecutor
    where T : CommandBase
{
    void Execute(T command);
}

public interface IExecutor
{
    Type CommandType { get; }

    void ExecuteCommandBase(CommandBase command);
}
