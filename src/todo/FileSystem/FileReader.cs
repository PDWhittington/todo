using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class FileReader : IFileReader
{
    public string GetFileText(string path)
    {
        if (!File.Exists(path)) throw new Exception($"{path} not found");

        return File.ReadAllText(path);
    }
}
