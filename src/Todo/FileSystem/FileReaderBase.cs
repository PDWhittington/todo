using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public abstract class FileReaderBase
{
    private readonly IUnmanagedByteArrayManager _unmanagedByteArrayManager;

    protected FileReaderBase(IUnmanagedByteArrayManager unmanagedByteArrayManager)
    {
        _unmanagedByteArrayManager = unmanagedByteArrayManager;
    }

    protected UnmanagedByteArray LoadFile(string fileName)
    {
        return _unmanagedByteArrayManager.LoadFromFile(fileName);
    }

    protected UnmanagedByteArray LoadFromManifest(string manifestName)
    {
        return _unmanagedByteArrayManager.LoadFromManifest(manifestName);
    }
}