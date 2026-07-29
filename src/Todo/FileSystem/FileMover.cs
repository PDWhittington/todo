using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class FileMover : IFileMover
{
    public void Move(string sourcePath, string destinationPath, bool allowOverwrite = false)
    {
        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException($"File {sourcePath} not found.");
        }

        if (File.Exists(destinationPath) && !allowOverwrite)
        {
            throw new Exception($"File {destinationPath} already present.");
        }

        File.Move(sourcePath, destinationPath);
    }
}
