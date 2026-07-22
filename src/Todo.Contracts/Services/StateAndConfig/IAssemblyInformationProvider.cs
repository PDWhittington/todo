namespace Todo.Contracts.Services.StateAndConfig;

public interface IAssemblyInformationProvider
{
    string GetCommitHash();

    DateTime GetBuildTime();

    string? GetMetadata(string key);

    string GetRootedToAssemblyFolder(string path);

    string AssemblyLocation();
    
    bool DebugFlag();
}
