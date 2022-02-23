using System;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

/// <summary>
/// This class
/// </summary>
public class CommandLineProvider : ICommandLineProvider
{
    private readonly IPathHelper _pathHelper;

    public CommandLineProvider(IPathHelper pathHelper)
    {
        _pathHelper = pathHelper;
    }

    public string GetCommandLineMinusAssemblyLocation()
    {
        var assemblyLocation = _pathHelper.GetAssemblyLocation();

        var wholeCommandLine = Environment.CommandLine;

        if (wholeCommandLine.StartsWith(assemblyLocation))
        {
            return wholeCommandLine[assemblyLocation.Length..]
                .Trim();
        }

        return wholeCommandLine.Trim();
    }
}
