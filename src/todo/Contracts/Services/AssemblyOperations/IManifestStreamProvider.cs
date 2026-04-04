namespace Todo.Contracts.Services.AssemblyOperations;

public interface IManifestStreamProvider
{
    byte[] GetBytesFromManifest(string manifestName);

    string GetStringFromManifest(string manifestName);

    string [] GetLinesFromManifest(string manifestName);

    void WriteStringFromManifestToFile(string manifestName, string path);
}
