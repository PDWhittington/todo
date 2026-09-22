using System;
using System.Text;

namespace Todo.FileSystem;

/// <summary>
/// Windows path rules based on the .NET runtime PathInternal.Windows / Path.Windows
/// implementation. GetFullPath is fully managed (no GetFullPathNameW).
/// </summary>
public class WindowsFileSystem : FileSystemBase
{
    private const char VolumeSeparator = ':';
    private const int DevicePrefixLength = 4;
    private const int UncPrefixLength = 2;
    private const int UncExtendedPrefixLength = 8;

    public override char DirectorySeparatorChar => '\\';

    public override char AltDirectorySeparatorChar => '/';

    public override char VolumeSeparatorChar => VolumeSeparator;

    public override char PathSeparator => ';';

    protected override StringComparison PathStringComparison => StringComparison.OrdinalIgnoreCase;

    public override bool IsPathRooted(string? path)
        => path != null && IsPathRooted(path.AsSpan());

    public override bool IsPathFullyQualified(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return !IsPartiallyQualified(path.AsSpan());
    }

    public override string GetFullPath(string path, string basePath)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(basePath);

        if (IsEffectivelyEmpty(path.AsSpan()))
            throw new ArgumentException("The path is empty.", nameof(path));

        if (!IsPathFullyQualified(basePath))
            throw new ArgumentException("Base path is not fully qualified.", nameof(basePath));

        if (basePath.Contains('\0') || path.Contains('\0'))
            throw new ArgumentException("Path contains a null character.");

        if (IsPathFullyQualified(path))
            return GetFullPathInternal(path);

        if (IsEffectivelyEmpty(path.AsSpan()))
            return basePath;

        string combinedPath;
        if (path.Length >= 1 && IsDirectorySeparator(path[0]))
        {
            // Current-drive rooted: "\Foo" + "C:\Bar" => "C:\Foo"
            var root = GetPathRootSpan(basePath.AsSpan());
            combinedPath = JoinInternal(root, path.AsSpan(1));
        }
        else if (path.Length >= 2 && IsValidDriveChar(path[0]) && path[1] == VolumeSeparator)
        {
            // Drive-relative: "C:Foo"
            if (GetDriveLetter(path.AsSpan()) == GetDriveLetter(basePath.AsSpan()))
                combinedPath = JoinInternal(basePath.AsSpan(), path.AsSpan(2));
            else
                combinedPath = path.Length == 2
                    ? string.Concat(path, DirectorySeparatorCharAsString)
                    : path.Insert(2, DirectorySeparatorCharAsString);
        }
        else
        {
            combinedPath = JoinInternal(basePath.AsSpan(), path.AsSpan());
        }

        return GetFullPathInternal(combinedPath);
    }

    protected override bool IsDirectorySeparator(char c)
        => c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;

    protected override int GetRootLength(ReadOnlySpan<char> path)
    {
        var pathLength = path.Length;
        var i = 0;

        var deviceSyntax = IsDevice(path);
        var deviceUnc = deviceSyntax && IsDeviceUnc(path);

        if ((!deviceSyntax || deviceUnc) && pathLength > 0 && IsDirectorySeparator(path[0]))
        {
            if (deviceUnc || (pathLength > 1 && IsDirectorySeparator(path[1])))
            {
                i = deviceUnc ? UncExtendedPrefixLength : UncPrefixLength;

                var n = 2;
                while (i < pathLength && (!IsDirectorySeparator(path[i]) || --n > 0))
                    i++;
            }
            else
            {
                i = 1;
            }
        }
        else if (deviceSyntax)
        {
            i = DevicePrefixLength;
            while (i < pathLength && !IsDirectorySeparator(path[i]))
                i++;

            if (i < pathLength && i > DevicePrefixLength && IsDirectorySeparator(path[i]))
                i++;
        }
        else if (pathLength >= 2
                 && path[1] == VolumeSeparator
                 && IsValidDriveChar(path[0]))
        {
            i = 2;
            if (pathLength > 2 && IsDirectorySeparator(path[2]))
                i++;
        }

        return i;
    }

    protected override bool IsPartiallyQualified(ReadOnlySpan<char> path)
    {
        if (path.Length < 2)
            return true;

        if (IsDirectorySeparator(path[0]))
            return !(path[1] == '?' || IsDirectorySeparator(path[1]));

        return !(path.Length >= 3
                 && path[1] == VolumeSeparator
                 && IsDirectorySeparator(path[2])
                 && IsValidDriveChar(path[0]));
    }

    protected override bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
    {
        if (path.IsEmpty)
            return true;

        foreach (var c in path)
        {
            if (c != ' ')
                return false;
        }

        return true;
    }

    protected override string NormalizeDirectorySeparators(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        var normalized = true;
        for (var i = 0; i < path.Length; i++)
        {
            var current = path[i];
            if (IsDirectorySeparator(current)
                && (current != DirectorySeparatorChar
                    || (i > 0 && i + 1 < path.Length && IsDirectorySeparator(path[i + 1]))))
            {
                normalized = false;
                break;
            }
        }

        if (normalized)
            return path;

        var builder = new StringBuilder(path.Length);

        var start = 0;
        if (IsDirectorySeparator(path[start]))
        {
            start++;
            builder.Append(DirectorySeparatorChar);
        }

        for (var i = start; i < path.Length; i++)
        {
            var current = path[i];

            if (IsDirectorySeparator(current))
            {
                if (i + 1 < path.Length && IsDirectorySeparator(path[i + 1]))
                    continue;

                current = DirectorySeparatorChar;
            }

            builder.Append(current);
        }

        return builder.ToString();
    }

    private static bool IsValidDriveChar(char value)
        => (uint)((value | 0x20) - 'a') <= 'z' - 'a';

    private bool IsPathRooted(ReadOnlySpan<char> path)
    {
        var length = path.Length;
        return (length >= 1 && IsDirectorySeparator(path[0]))
               || (length >= 2 && IsValidDriveChar(path[0]) && path[1] == VolumeSeparator);
    }

    private static bool IsExtended(ReadOnlySpan<char> path)
        => path.Length >= DevicePrefixLength
           && path[0] == '\\'
           && (path[1] == '\\' || path[1] == '?')
           && path[2] == '?'
           && path[3] == '\\';

    private bool IsDevice(ReadOnlySpan<char> path)
        => IsExtended(path)
           || (path.Length >= DevicePrefixLength
               && IsDirectorySeparator(path[0])
               && IsDirectorySeparator(path[1])
               && (path[2] == '.' || path[2] == '?')
               && IsDirectorySeparator(path[3]));

    private bool IsDeviceUnc(ReadOnlySpan<char> path)
        => path.Length >= UncExtendedPrefixLength
           && IsDevice(path)
           && IsDirectorySeparator(path[7])
           && path[4] == 'U'
           && path[5] == 'N'
           && path[6] == 'C';

    private static char? GetDriveLetter(ReadOnlySpan<char> path)
    {
        for (var i = 0; i < path.Length - 1; i++)
        {
            if (path[i + 1] == VolumeSeparator && IsValidDriveChar(path[i]))
                return char.ToUpperInvariant(path[i]);
        }

        return null;
    }

    private string GetFullPathInternal(string path)
    {
        if (IsExtended(path.AsSpan()))
            return path;

        var collapsed = RemoveRelativeSegments(path, GetRootLength(path));
        return NormalizeDirectorySeparators(collapsed);
    }
}
