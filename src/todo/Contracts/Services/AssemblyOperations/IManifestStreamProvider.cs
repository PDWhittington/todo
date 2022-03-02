namespace Todo.Contracts.Services.AssemblyOperations;

public interface IManifestStreamProvider
{
    string GetStringFromManifest(string manifestName);
}
