using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record PerOsFilePaths(
    ProcessLaunchInfo Windows,
    ProcessLaunchInfo Linux,
    // ReSharper disable once InconsistentNaming
    ProcessLaunchInfo OSX)
{
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
