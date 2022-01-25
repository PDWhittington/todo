namespace Todo.Contracts.Data.FileSystem;

public class TodoFile
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public FilePathInfo FilePathInfo { get; }

    public string FileContents { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    public TodoFile(FilePathInfo filePathInfo, string fileContents)
    {
        FilePathInfo = filePathInfo;
        FileContents = fileContents;
    }

    public static TodoFile Of(FilePathInfo filePathInfo, string fileContents) => new(filePathInfo, fileContents);
}
