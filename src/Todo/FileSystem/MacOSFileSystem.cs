using System;

namespace Todo.FileSystem;

public class MacOSFileSystem : UnixFileSystem
{
    protected override StringComparison PathStringComparison => StringComparison.OrdinalIgnoreCase;
}
