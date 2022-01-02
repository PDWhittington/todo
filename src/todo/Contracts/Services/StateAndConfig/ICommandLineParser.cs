using System;

namespace Todo.Contracts.Services.StateAndConfig
{
    public interface ICommandLineParser
    {
        DateOnly GetDateFromCommandLine();

        bool TryGetWordFromCommandLine(string[] candidates, out string word);

        string GetCommandLineMinusAssemblyLocation();
    }
}