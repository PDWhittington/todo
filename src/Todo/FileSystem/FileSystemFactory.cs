using System;
using System.Runtime.InteropServices;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

public class FileSystemFactory : IFileSystemFactory
{
    private readonly Lazy<IFileSystem> _fileSystem = new(CreateForCurrentOs);

    public IFileSystem Create() => _fileSystem.Value;

    private static IFileSystem CreateForCurrentOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new WindowsFileSystem();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new MacOSFileSystem();

        return new LinuxFileSystem();
    }
}
