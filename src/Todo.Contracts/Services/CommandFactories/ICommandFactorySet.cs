using System.Collections.Generic;
using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.CommandFactories;

public interface ICommandFactorySet
{
    ICommandFactory<CommandBase> DefaultCommandFactory { get; }
    ICommandFactory<CommandBase>[] NonDefaultCommandFactories { get; }

    IEnumerable<ICommandFactory<CommandBase>> GetAllCommandFactories();
}
