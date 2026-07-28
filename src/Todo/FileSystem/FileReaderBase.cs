using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public abstract class FileReaderBase(IUnmanagedByteArrayManager unmanagedByteArrayManager)
{
    protected UnmanagedByteArray LoadFile(string fileName)
    {
        return unmanagedByteArrayManager.LoadFromFile(fileName);
    }

    protected UnmanagedByteArray LoadFromManifest(string manifestName)
    {
        return unmanagedByteArrayManager.LoadFromManifest(manifestName);
    }
}
