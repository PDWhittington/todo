using System;
using System.IO;

namespace Todo.FileSystem;

public abstract class FileReaderBase
{
    protected static string GetFileText(string path)
    {
        if (!File.Exists(path)) throw new Exception($"{path} not found");

        return File.ReadAllText(path);
    }
}
