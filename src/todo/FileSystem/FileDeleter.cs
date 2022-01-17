using System.IO;
using Todo.Contracts.Data.FileSystem;

namespace Todo.FileSystem;

public class FileDeleter : IFileDeleter
{
    public void Delete(string folder, string fileOrWildCard)
    {
        var dir = new DirectoryInfo(folder);

        foreach (var file in dir.EnumerateFiles(fileOrWildCard)) {
            file.Delete();
        }
    }
}
