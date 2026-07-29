namespace Todo.Contracts.Services.FileSystem;

public interface IFileMover
{
    void Move(string sourcePath, string destinationPath, bool allowOverwrite = false);
}
