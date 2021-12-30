using System;

namespace Todo.Contracts.Services
{
    public interface ICommandLineParser
    {
        DateOnly GetDateFromCommandLine();
    }
}