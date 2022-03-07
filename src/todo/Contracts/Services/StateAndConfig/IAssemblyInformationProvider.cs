using System;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IAssemblyInformationProvider
{
    string GetCommitHash();

    DateTime GetBuildTime();

    string AssemblyLocation();
}
