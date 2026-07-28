using Todo.Contracts.Data.Commands;

namespace Todo.Contracts.Services.CommandFactories;

public interface ICommandFactory<out T> where T : CommandBase
{
    T? TryGetCommand(string commandLine);

    bool IsDefaultCommandFactory { get; }

    public IEnumerable<string> GetFullHelpMessage();

    HashSet<string> CommandWords { get; }
}