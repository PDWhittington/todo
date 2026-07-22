using Todo.Contracts.Data.Config;

namespace Todo.Contracts.Services.StateAndConfig;

public interface IAssemblyInformationProvider
{
    PackageReferenceInfo[] GetPackageReferences();
    
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
