using System.Diagnostics.CodeAnalysis;

namespace Todo.Contracts.Services.FileSystem;

/// <summary>
/// OS-specific path manipulation, matching the behaviour of System.IO.Path
/// on that operating system. Implementations contain the path logic themselves
/// rather than forwarding to System.IO.Path.
/// </summary>
public interface IFileSystem
{
    char DirectorySeparatorChar { get; }

    char AltDirectorySeparatorChar { get; }

    char VolumeSeparatorChar { get; }

    char PathSeparator { get; }

    bool IsPathRooted(string? path);

    bool IsPathFullyQualified(string path);

    string Combine(string path1, string path2);

    string Combine(params string[] paths);

    string GetFullPath(string path);

    string GetFullPath(string path, string basePath);

    string? GetDirectoryName(string? path);

    [return: NotNullIfNotNull(nameof(path))]
    string? GetFileName(string? path);

    [return: NotNullIfNotNull(nameof(path))]
    string? GetFileNameWithoutExtension(string? path);

    string GetRelativePath(string relativeTo, string path);
}
