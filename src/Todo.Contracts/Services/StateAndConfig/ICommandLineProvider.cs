using Todo.Contracts.Data.CommandLine;

namespace Todo.Contracts.Services.StateAndConfig;

public interface ICommandLineProvider
{
    CommandLineInfo GetCommandLine();
}
