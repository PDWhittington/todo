using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Todo.Contracts.Services.FileSystem;

namespace Todo.FileSystem;

/// <summary>
/// Shared path algorithms from the .NET runtime Path / PathInternal implementation,
/// parameterised by OS-specific separator and rooting rules.
/// </summary>
public abstract class FileSystemBase : IFileSystem
{
    public abstract char DirectorySeparatorChar { get; }

    public abstract char AltDirectorySeparatorChar { get; }

    public abstract char VolumeSeparatorChar { get; }

    public abstract char PathSeparator { get; }

    protected abstract StringComparison PathStringComparison { get; }

    protected string DirectorySeparatorCharAsString => DirectorySeparatorChar.ToString();

    public abstract bool IsPathRooted(string? path);

    public abstract bool IsPathFullyQualified(string path);

    protected abstract bool IsDirectorySeparator(char c);

    protected abstract int GetRootLength(ReadOnlySpan<char> path);

    protected abstract bool IsPartiallyQualified(ReadOnlySpan<char> path);

    protected abstract bool IsEffectivelyEmpty(ReadOnlySpan<char> path);

    public string Combine(string path1, string path2)
    {
        ArgumentNullException.ThrowIfNull(path1);
        ArgumentNullException.ThrowIfNull(path2);

        return CombineInternal(path1, path2);
    }

    public string Combine(params string[] paths)
    {
        ArgumentNullException.ThrowIfNull(paths);

        var firstComponent = 0;
        for (var i = 0; i < paths.Length; i++)
        {
            ArgumentNullException.ThrowIfNull(paths[i], nameof(paths));

            if (paths[i].Length == 0)
                continue;

            if (IsPathRooted(paths[i]))
                firstComponent = i;
        }

        var builder = new StringBuilder();

        for (var i = firstComponent; i < paths.Length; i++)
        {
            if (paths[i].Length == 0)
                continue;

            if (builder.Length == 0)
            {
                builder.Append(paths[i]);
            }
            else
            {
                if (!IsDirectorySeparator(builder[^1]))
                    builder.Append(DirectorySeparatorChar);

                builder.Append(paths[i]);
            }
        }

        return builder.ToString();
    }

    public string GetFullPath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        // Already-qualified paths must not depend on the host current directory, so a
        // WindowsFileSystem can still normalise "C:\..." when tests run on Unix (and vice versa).
        if (IsPathFullyQualified(path))
            return GetFullPath(path, path);

        return GetFullPath(path, GetCurrentDirectory());
    }

    public abstract string GetFullPath(string path, string basePath);

    public string? GetDirectoryName(string? path)
    {
        if (path == null || IsEffectivelyEmpty(path.AsSpan()))
            return null;

        var end = GetDirectoryNameOffset(path.AsSpan());
        return end >= 0 ? NormalizeDirectorySeparators(path[..end]) : null;
    }

    [return: NotNullIfNotNull(nameof(path))]
    public string? GetFileName(string? path)
    {
        if (path == null)
            return null;

        var result = GetFileName(path.AsSpan());
        return path.Length == result.Length ? path : result.ToString();
    }

    [return: NotNullIfNotNull(nameof(path))]
    public string? GetFileNameWithoutExtension(string? path)
    {
        if (path == null)
            return null;

        var fileName = GetFileName(path.AsSpan());
        var lastPeriod = fileName.LastIndexOf('.');
        var result = lastPeriod < 0 ? fileName : fileName[..lastPeriod];
        return path.Length == result.Length ? path : result.ToString();
    }

    public string GetRelativePath(string relativeTo, string path)
    {
        ArgumentNullException.ThrowIfNull(relativeTo);
        ArgumentNullException.ThrowIfNull(path);

        if (IsEffectivelyEmpty(relativeTo.AsSpan()))
            throw new ArgumentException("The path is empty.", nameof(relativeTo));
        if (IsEffectivelyEmpty(path.AsSpan()))
            throw new ArgumentException("The path is empty.", nameof(path));

        relativeTo = GetFullPath(relativeTo);
        path = GetFullPath(path);

        if (!AreRootsEqual(relativeTo, path, PathStringComparison))
            return path;

        var ignoreCase = PathStringComparison == StringComparison.OrdinalIgnoreCase;
        var commonLength = GetCommonPathLength(relativeTo, path, ignoreCase);

        if (commonLength == 0)
            return path;

        var relativeToLength = relativeTo.Length;
        if (EndsInDirectorySeparator(relativeTo.AsSpan()))
            relativeToLength--;

        var pathEndsInSeparator = EndsInDirectorySeparator(path.AsSpan());
        var pathLength = path.Length;
        if (pathEndsInSeparator)
            pathLength--;

        if (relativeToLength == pathLength && commonLength >= relativeToLength)
            return ".";

        var sb = new StringBuilder(Math.Max(relativeTo.Length, path.Length));

        if (commonLength < relativeToLength)
        {
            sb.Append("..");

            for (var i = commonLength + 1; i < relativeToLength; i++)
            {
                if (IsDirectorySeparator(relativeTo[i]))
                {
                    sb.Append(DirectorySeparatorChar);
                    sb.Append("..");
                }
            }
        }
        else if (IsDirectorySeparator(path[commonLength]))
        {
            commonLength++;
        }

        var differenceLength = pathLength - commonLength;
        if (pathEndsInSeparator)
            differenceLength++;

        if (differenceLength > 0)
        {
            if (sb.Length > 0)
                sb.Append(DirectorySeparatorChar);

            sb.Append(path.AsSpan(commonLength, differenceLength));
        }

        return sb.ToString();
    }

    protected virtual string GetCurrentDirectory() => Environment.CurrentDirectory;

    protected string CombineInternal(string first, string second)
    {
        if (string.IsNullOrEmpty(first))
            return second;

        if (string.IsNullOrEmpty(second))
            return first;

        if (IsPathRooted(second))
            return second;

        return JoinInternal(first.AsSpan(), second.AsSpan());
    }

    protected string JoinInternal(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        var hasSeparator = IsDirectorySeparator(first[^1]) || IsDirectorySeparator(second[0]);

        return hasSeparator
            ? string.Concat(first, second)
            : string.Concat(first, DirectorySeparatorCharAsString, second);
    }

    protected ReadOnlySpan<char> GetPathRootSpan(ReadOnlySpan<char> path)
    {
        if (IsEffectivelyEmpty(path))
            return ReadOnlySpan<char>.Empty;

        var rootLength = GetRootLength(path);
        return rootLength <= 0 ? ReadOnlySpan<char>.Empty : path[..rootLength];
    }

    protected int GetDirectoryNameOffset(ReadOnlySpan<char> path)
    {
        var rootLength = GetRootLength(path);
        var end = path.Length;
        if (end <= rootLength)
            return -1;

        while (end > rootLength && !IsDirectorySeparator(path[--end])) { }

        while (end > rootLength && IsDirectorySeparator(path[end - 1]))
            end--;

        return end;
    }

    protected ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
    {
        var root = GetPathRootSpan(path).Length;

        var i = DirectorySeparatorChar == AltDirectorySeparatorChar
            ? path.LastIndexOf(DirectorySeparatorChar)
            : path.LastIndexOfAny(DirectorySeparatorChar, AltDirectorySeparatorChar);

        return path[(i < root ? root : i + 1)..];
    }

    protected bool EndsInDirectorySeparator(ReadOnlySpan<char> path)
        => path.Length > 0 && IsDirectorySeparator(path[^1]);

    protected abstract string NormalizeDirectorySeparators(string path);

    protected string RemoveRelativeSegments(string path, int rootLength)
    {
        if (rootLength < 0)
            rootLength = 0;

        var sb = new StringBuilder(path.Length);
        var skip = rootLength;

        if (skip > 0 && IsDirectorySeparator(path[skip - 1]))
            skip--;

        var flippedSeparator = false;

        if (skip > 0)
            sb.Append(path.AsSpan(0, skip));

        for (var i = skip; i < path.Length; i++)
        {
            var c = path[i];

            if (IsDirectorySeparator(c) && i + 1 < path.Length)
            {
                if (IsDirectorySeparator(path[i + 1]))
                    continue;

                if ((i + 2 == path.Length || IsDirectorySeparator(path[i + 2])) &&
                    path[i + 1] == '.')
                {
                    i++;
                    continue;
                }

                if (i + 2 < path.Length &&
                    (i + 3 == path.Length || IsDirectorySeparator(path[i + 3])) &&
                    path[i + 1] == '.' && path[i + 2] == '.')
                {
                    var s = sb.Length - 1;
                    for (; s >= skip; s--)
                    {
                        if (!IsDirectorySeparator(sb[s]))
                            continue;

                        sb.Length = i + 3 >= path.Length && s == skip ? s + 1 : s;
                        break;
                    }

                    if (s < skip)
                        sb.Length = skip;

                    i += 2;
                    continue;
                }
            }

            if (c != DirectorySeparatorChar && c == AltDirectorySeparatorChar)
            {
                c = DirectorySeparatorChar;
                flippedSeparator = true;
            }

            sb.Append(c);
        }

        if (!flippedSeparator && sb.Length == path.Length)
            return path;

        if (skip != rootLength && sb.Length < rootLength)
            sb.Append(path[rootLength - 1]);

        return sb.ToString();
    }

    private bool AreRootsEqual(string first, string second, StringComparison comparisonType)
    {
        var firstRootLength = GetRootLength(first.AsSpan());
        var secondRootLength = GetRootLength(second.AsSpan());

        return firstRootLength == secondRootLength
               && string.Compare(first, 0, second, 0, firstRootLength, comparisonType) == 0;
    }

    private int GetCommonPathLength(string first, string second, bool ignoreCase)
    {
        var commonChars = EqualStartingCharacterCount(first, second, ignoreCase);

        if (commonChars == 0)
            return commonChars;

        if (commonChars == first.Length
            && (commonChars == second.Length || IsDirectorySeparator(second[commonChars])))
            return commonChars;

        if (commonChars == second.Length && IsDirectorySeparator(first[commonChars]))
            return commonChars;

        while (commonChars > 0 && !IsDirectorySeparator(first[commonChars - 1]))
            commonChars--;

        return commonChars;
    }

    private static int EqualStartingCharacterCount(string first, string second, bool ignoreCase)
    {
        var max = Math.Min(first.Length, second.Length);
        var i = 0;

        for (; i < max; i++)
        {
            if (ignoreCase)
            {
                if (char.ToUpperInvariant(first[i]) != char.ToUpperInvariant(second[i]))
                    break;
            }
            else if (first[i] != second[i])
            {
                break;
            }
        }

        return i;
    }
}
