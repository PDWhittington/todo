using System;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.StateAndConfig;

public class CommandLineProvider : ICommandLineProvider
{
    private readonly IAssemblyInformationProvider _assemblyInformationProvider;
    private readonly Lazy<string> _commandLine;

    public CommandLineProvider(IAssemblyInformationProvider assemblyInformationProvider)
    {
        _assemblyInformationProvider = assemblyInformationProvider;
        _commandLine = new Lazy<string>(GenerateCommandLineMinusAssemblyLocation);
    }
        
    public string GetCommandLineMinusAssemblyLocation() => _commandLine.Value;
        
    private string GenerateCommandLineMinusAssemblyLocation()
    {
        var assemblyLocation = _assemblyInformationProvider.AssemblyLocation();

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
