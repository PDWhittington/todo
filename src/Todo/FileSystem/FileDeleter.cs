using System.IO;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.UI;

namespace Todo.FileSystem;

public class FileDeleter(IOutputWriter outputWriter) : IFileDeleter
{
    public void Delete(string folder, string fileOrWildCard)
    {
        var dir = new DirectoryInfo(folder);

        foreach (var file in dir.EnumerateFiles(fileOrWildCard))
        {
            outputWriter.WriteLine($"Deleting {file.FullName}");
            file.Delete();
        }
    }
}
