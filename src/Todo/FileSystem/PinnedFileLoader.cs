using System;
using System.IO;
using System.Runtime.InteropServices;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public sealed class PinnedFileLoader : IPinnedFileLoader
{
    public UnmanagedByteArray LoadFile(string filePath)
    {
        // 1. Get exact size (fast)
        var fileLength = new FileInfo(filePath).Length;
        if (fileLength > int.MaxValue)
            throw new NotSupportedException("File too large for byte[]");

        // 2. Allocate directly on the Pinned Object Heap (best for long-lived pins)
        var data  = GC.AllocateArray<byte>((int)fileLength, pinned: true);

        // 3. Read the file as fast as possible into the pre-allocated buffer
        using var fs = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,           // large buffer = fewer syscalls
            FileOptions.SequentialScan);     // hint to OS + .NET for max throughput

        fs.ReadExactly(data);               // .NET 5+ - guaranteed full read, zero extra copies

        // 4. Permanently pin and get a stable pointer
        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        var pointer = handle.AddrOfPinnedObject();
        
        return UnmanagedByteArray.Of(handle, pointer, (int)fileLength);
    }
}