using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Services.AssemblyOperations;

public interface IManifestStreamProvider
{
    // ReSharper disable once UnusedMemberInSuper.Global
    byte[] GetBytesFromManifest(string manifestName);

    UnmanagedByteArray LoadFromManifest(string manifestName);

    string GetStringFromManifest(string manifestName);

    void WriteStringFromManifestToFile(string manifestName, string path);
}