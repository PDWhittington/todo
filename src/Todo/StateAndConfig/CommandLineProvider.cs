using System;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class CommandLineProvider : ICommandLineProvider
{
    private readonly Lazy<CommandLineInfo> _commandLine = new(ParseCommandLine);

    public CommandLineInfo GetCommandLine() => _commandLine.Value;

    private static CommandLineInfo ParseCommandLine()
        => CommandLineInfo.FromArgs(Environment.GetCommandLineArgs());
}
