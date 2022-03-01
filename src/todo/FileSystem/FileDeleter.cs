using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Reporting;

namespace Todo.FileSystem;

public class FileDeleter : IFileDeleter
{
    private readonly IOutputWriter _outputWriter;

    public FileDeleter(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter;
    }

    public void Delete(string folder, string fileOrWildCard)
    {
        var dir = new DirectoryInfo(folder);

        foreach (var file in dir.EnumerateFiles(fileOrWildCard)) {
            _outputWriter.WriteLine($"Deleting {file.FullName}");
            file.Delete();
        }
    }
}
