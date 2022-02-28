using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

public class PerOsFilePaths
{
    public ProcessLaunchInfo Windows { get; }
    public ProcessLaunchInfo Linux { get; }
    public ProcessLaunchInfo OSX { get; }

    [JsonConstructor]
    public PerOsFilePaths(ProcessLaunchInfo windows, 
        ProcessLaunchInfo linux, ProcessLaunchInfo osx)
    {
        Windows = windows;
        Linux = linux;
        OSX = osx;
    }

    public ProcessLaunchInfo GetPathForThisOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Windows;
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? OSX
            : Linux; // Assume that any unrecognised OS is a POSIX variant.
    }
}
