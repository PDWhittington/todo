using System;

namespace Todo.Contracts.Services.StateAndConfig
{
    public interface ICommandLineProvider
    {
        DateOnly GetDateFromCommandLine();

        bool TryGetWordFromCommandLine(string[] candidates, out string word);

        string GetCommandLineMinusAssemblyLocation();
    }
}