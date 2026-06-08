namespace Todo.Contracts.Data.FileSystem;

public static class FileSystemExtensions
{
    public static bool Exists(this FilePathInfo filePathInfo) => File.Exists(filePathInfo.Path);
}