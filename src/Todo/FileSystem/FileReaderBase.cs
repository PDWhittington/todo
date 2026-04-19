using System;
using System.IO;

namespace Todo.FileSystem;

public abstract class FileReaderBase
{
    protected static byte[] GetFileBytes(string path)
    {
        if (!File.Exists(path)) throw new Exception($"{path} not found");

        var allBytes = File.ReadAllBytes(path);
        return allBytes;
    }
}
