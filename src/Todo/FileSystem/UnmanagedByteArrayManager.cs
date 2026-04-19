using System.Collections.Generic;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class UnmanagedByteArrayManager(IPinnedFileLoader pinnedFileLoader, 
    IManifestStreamProvider manifestStreamProvider) : IUnmanagedByteArrayManager
{
    private readonly List<UnmanagedByteArray> _files = new(1000);

    public UnmanagedByteArray LoadFromFile(string fileName)
    {
        var file = pinnedFileLoader.LoadFile(fileName);
        _files.Add(file);
        return file;
    }

    public UnmanagedByteArray LoadFromManifest(string manifestName)
    {
        var file = manifestStreamProvider.LoadFromManifest(manifestName);
        _files.Add(file);
        return file;
    }

    ~UnmanagedByteArrayManager()
    {
        foreach (var file in _files)
        {
            file.Handle.Free();
        }
    }
}