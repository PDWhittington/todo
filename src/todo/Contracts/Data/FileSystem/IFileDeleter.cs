namespace Todo.Contracts.Data.FileSystem;

public interface IFileDeleter
{
    void Delete(string folder, string fileOrWildCard);
}
