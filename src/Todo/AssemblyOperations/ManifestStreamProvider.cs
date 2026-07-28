using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.AssemblyOperations;

namespace Todo.AssemblyOperations;

public class ManifestStreamProvider : IManifestStreamProvider
{
    public byte[] GetBytesFromManifest(string manifestName)
    {
        var manifestStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(manifestName);

        if (manifestStream is null) throw new Exception(
            $"Manifest with name {manifestName} not found in assembly");

        var buffer = new byte[manifestStream.Length];

        manifestStream.ReadExactly(buffer);
        return buffer;
    }

    public UnmanagedByteArray LoadFromManifest(string manifestName)
    {
        var manifestStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(manifestName);

        if  (manifestStream is null) throw new Exception("Manifest not found in assembly");

        var length = (int)manifestStream.Length;

        // 2. Allocate directly on the Pinned Object Heap (best for long-lived pins)
        var data  = GC.AllocateArray<byte>(length, pinned: true);

        manifestStream.ReadExactly(data);

        // 4. Permanently pin and get a stable pointer
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        var pointer = handle.AddrOfPinnedObject();

        return UnmanagedByteArray.Of(handle, pointer, length);
    }

    public string GetStringFromManifest(string manifestName)
    {
        var buffer = GetBytesFromManifest(manifestName);

        var text = Encoding.UTF8.GetString(buffer);
        return text;
    }

    public void WriteStringFromManifestToFile(string manifestName, string path)
    {
        var buffer = GetBytesFromManifest(manifestName);

        using var file = new FileStream(path, FileMode.Create, FileAccess.Write);
        file.Write(buffer, 0, buffer.Length);
    }
}