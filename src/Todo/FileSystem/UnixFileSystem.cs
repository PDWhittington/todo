using System;
using System.Text;

namespace Todo.FileSystem;

/// <summary>
/// POSIX path rules shared by Linux and macOS, based on the .NET runtime
/// PathInternal.Unix / Path.Unix implementation.
/// </summary>
public abstract class UnixFileSystem : FileSystemBase
{
    public override char DirectorySeparatorChar => '/';

    public override char AltDirectorySeparatorChar => '/';

    public override char VolumeSeparatorChar => '/';

    public override char PathSeparator => ':';

    public override bool IsPathRooted(string? path)
        => path != null && IsPathRooted(path.AsSpan());

    public override bool IsPathFullyQualified(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        return IsPathRooted(path);
    }

    public override string GetFullPath(string path, string basePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        ArgumentNullException.ThrowIfNull(basePath);

        if (!IsPathFullyQualified(basePath))
            throw new ArgumentException("Base path is not fully qualified.", nameof(basePath));

        if (basePath.Contains('\0') || path.Contains('\0'))
            throw new ArgumentException("Path contains a null character.");

        if (IsPathFullyQualified(path))
            return GetFullPathInternal(path);

        return GetFullPathInternal(CombineInternal(basePath, path));
    }

    protected override bool IsDirectorySeparator(char c) => c == '/';

    protected override int GetRootLength(ReadOnlySpan<char> path)
        => path.Length > 0 && IsDirectorySeparator(path[0]) ? 1 : 0;

    protected override bool IsPartiallyQualified(ReadOnlySpan<char> path)
        => !IsPathRooted(path);

    protected override bool IsEffectivelyEmpty(ReadOnlySpan<char> path)
        => path.IsEmpty;

    protected override string NormalizeDirectorySeparators(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        var normalized = true;
        for (var i = 0; i < path.Length; i++)
        {
            if (IsDirectorySeparator(path[i])
                && i + 1 < path.Length && IsDirectorySeparator(path[i + 1]))
            {
                normalized = false;
                break;
            }
        }

        if (normalized)
            return path;

        var builder = new StringBuilder(path.Length);
        for (var i = 0; i < path.Length; i++)
        {
            var current = path[i];
            if (IsDirectorySeparator(current)
                && i + 1 < path.Length && IsDirectorySeparator(path[i + 1]))
                continue;

            builder.Append(current);
        }

        return builder.ToString();
    }

    private bool IsPathRooted(ReadOnlySpan<char> path)
        => path.Length > 0 && path[0] == DirectorySeparatorChar;

    private string GetFullPathInternal(string path)
    {
        if (!IsPathRooted(path))
            path = CombineInternal(GetCurrentDirectory(), path);

        var collapsed = RemoveRelativeSegments(path, GetRootLength(path));
        return collapsed.Length == 0 ? DirectorySeparatorCharAsString : collapsed;
    }
}
