using System;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

/// <summary>
/// This class
/// </summary>
public class CommandLineProvider(IPathHelper pathHelper) : ICommandLineProvider
{
    public string GetCommandLineMinusAssemblyLocation()
    {
        var assemblyLocation = pathHelper.GetAssemblyLocation();

        var wholeCommandLine = Environment.CommandLine;

        if (wholeCommandLine.StartsWith(assemblyLocation,  StringComparison.OrdinalIgnoreCase))
        {
            return wholeCommandLine[assemblyLocation.Length..]
                .Trim();
        }

        if (wholeCommandLine.StartsWith("todo", StringComparison.OrdinalIgnoreCase))
        {
            return wholeCommandLine["todo".Length..].Trim();
        }

        return wholeCommandLine.Trim();
    }
}
