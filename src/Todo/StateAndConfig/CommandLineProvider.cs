using System;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

/// <summary>
/// This class
/// </summary>
public class CommandLineProvider(IAssemblyInformationProvider assemblyInformationProvider) 
    : ICommandLineProvider
{
    public string GetCommandLineMinusAssemblyLocation()
    {
        var assemblyLocation = assemblyInformationProvider.AssemblyLocation();

        var wholeCommandLine = Environment.CommandLine;

        if (wholeCommandLine.StartsWith(assemblyLocation,  StringComparison.OrdinalIgnoreCase))
        {
            return wholeCommandLine[assemblyLocation.Length..]
                .Trim();
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (wholeCommandLine.StartsWith("todo", StringComparison.OrdinalIgnoreCase))
        {
            return wholeCommandLine["todo".Length..].Trim();
        }

        return wholeCommandLine.Trim();
    }
}
