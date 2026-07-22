namespace Todo.Contracts.Services.StateAndConfig;

public interface IAssemblyInformationProvider
{
    string GitDescribe();

    string [] GitBranches();
    
    string [] GitTags();

    string [] GitWorktreeChanges();

    DateTime GetBuildTime();

    string? GetMetadata(string key);

    string GetRootedToAssemblyFolder(string path);

    string AssemblyLocation();
    
    bool DebugFlag();
}
