using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Services.FileSystem;

public interface IUnmanagedByteArrayManager
{
    UnmanagedByteArray LoadFromFile(string fileName);

    UnmanagedByteArray LoadFromManifest(string manifestName);
}