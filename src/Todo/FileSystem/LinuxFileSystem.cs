using System;

namespace Todo.FileSystem;

public class LinuxFileSystem : UnixFileSystem
{
    protected override StringComparison PathStringComparison => StringComparison.Ordinal;
}
