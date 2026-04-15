using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record PerOsFilePaths(
    ProcessLaunchInfo Windows,
    ProcessLaunchInfo Linux,
    ProcessLaunchInfo Osx)
{
    public ProcessLaunchInfo GetPathForThisOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Windows;
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            ? Osx
            : Linux; // Assume that any unrecognised OS is a POSIX variant.
    }
}
