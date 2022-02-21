using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data;

public class PerOsFilePaths
{
    public string WindowsPath { get; }
    public string LinuxPath { get; }
    public string OSXPath { get; }

    [JsonConstructor]
    public PerOsFilePaths(string windowsPath, string linuxPath, string osxPath)
    {
        WindowsPath = windowsPath;
        LinuxPath = linuxPath;
        OSXPath = osxPath;
    }

    public string GetPathForThisOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return WindowsPath;
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? OSXPath
            : LinuxPath; // Assume that any unrecognised OS is a POSIX variant.
    }
}
