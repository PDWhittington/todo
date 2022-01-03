namespace Todo.Contracts.Services.FileSystem;

public interface IFileReader
{
     string GetFileText(string path);
}
