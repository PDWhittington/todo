using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

public class PerOsFilePaths
{
    // ReSharper disable once MemberCanBePrivate.Global
    public ProcessLaunchInfo Windows { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    public ProcessLaunchInfo Linux { get; }

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
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
