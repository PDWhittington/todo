namespace Todo.Contracts.Services.FileSystem;

public interface IFileDeleter
{
    void Delete(string folder, string fileOrWildCard);
}
