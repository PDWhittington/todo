namespace Todo.Contracts.Data.FileSystem;

public class TodoFile
{
    public FilePathInfo FilePathInfo { get; }

    public string FileContents { get; }

    public TodoFile(FilePathInfo filePathInfo, string fileContents)
    {
        FilePathInfo = filePathInfo;
        FileContents = fileContents;
    }

    public static TodoFile Of(FilePathInfo filePathInfo, string fileContents) => new(filePathInfo, fileContents);
}
