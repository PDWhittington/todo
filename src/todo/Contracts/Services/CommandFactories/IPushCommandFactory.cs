using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.CommandFactories;

public interface IPushCommandFactory : ICommandFactory<PushCommand> { }
